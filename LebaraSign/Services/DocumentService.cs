using Azure;
using Azure.Storage.Blobs;
using LebaraSign.Models;
using LebaraSign.Services.Storage;
using System.Net;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using PdfDocument = UglyToad.PdfPig.PdfDocument;


namespace LebaraSign.Services;

public class DocumentService : IDocumentService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ITableService _tableService;

    public DocumentService(BlobServiceClient blobServiceClient, ITableService tableService)
    {
        _blobServiceClient = blobServiceClient;
        _tableService = tableService;
    }
    public async Task<AzureBlobResponse> UploadFileAsync(FileUploadModel fileUpload)
    {        
        var response = new AzureBlobResponse();
        var guid = Guid.NewGuid().ToString();
        var searchText = "verbal";
        //var signTag = "\\sn1\\"; // need to use to identify signtag in PDF for future
        var path = $"{Directory.GetCurrentDirectory()}\\Certificate\\certificate.pfx";
        var certPassword = "Mahadev@321#"; // future get from KeyVault
        try
        {
            var container = _blobServiceClient.GetBlobContainerClient("lebarasign");
            await container.CreateIfNotExistsAsync();

            using (var pdfStream = fileUpload.ContractPdf.OpenReadStream())
            using (var imageStream = fileUpload.SignatureImage.OpenReadStream())
            using (var modifiedPdfStream = new MemoryStream())
            {
                pdfStream.Position = 0;
                imageStream.Position = 0;
                // Open PDF using PDFPig to search text
                using (var pdfPigDocument = PdfDocument.Open(pdfStream))
                {                   
                    PdfSharp.Pdf.PdfDocument pdfSharpDocument = PdfSharp.Pdf.IO.PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify);
                    bool textFound = false;

                    foreach (var page in pdfPigDocument.GetPages())
                    {
                        var words = page.GetWords();
                        var foundWord = words.FirstOrDefault(w => w.Text.Contains(searchText, StringComparison.OrdinalIgnoreCase));

                        if (foundWord != null)
                        {
                            int pageIndex = page.Number - 1;
                            var pdfSharpPage = pdfSharpDocument.Pages[pageIndex];
                            var graphics = XGraphics.FromPdfPage(pdfSharpPage);

                            double xPosition = foundWord.BoundingBox.Left + 50;
                            double yPosition = (pdfSharpPage.Height - foundWord.BoundingBox.Top) + 50;

                            Console.WriteLine($"Embedding signature on page {pageIndex + 1} at coordinates ({xPosition}, {yPosition})");

                            imageStream.Position = 0;
                            XImage signatureImage = XImage.FromStream(imageStream);
                            graphics.DrawImage(signatureImage, xPosition, yPosition, 200, 50);

                            textFound = true;
                            break; // Embed on the first found instance
                        }
                    }

                    if (!textFound)
                    {
                        Console.WriteLine("Search text not found in the PDF.");
                        response.Id = guid;
                        response.IsError = true;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        response.ErrorMessage = "Search text not found in the PDF.";
                    }

                    // Save modified PDF to memory stream
                    pdfSharpDocument.Save(modifiedPdfStream);
                }            
                
                //digital signing using iText7 library
                var signedPdfStream = new MemoryStream();
                modifiedPdfStream.Position = 0;

                // Upload modified PDF to Azure Blob Storage
                signedPdfStream.Position = 0;
                pdfStream.Position = 0;
                imageStream.Position = 0;
                var signContractBlob = container.GetBlobClient($"signedcontract/{guid}.pdf");
                var signContractResult = await signContractBlob.UploadAsync(signedPdfStream);
                Console.WriteLine("PDF with signature uploaded successfully.");
                                
                var contractBlob = container.GetBlobClient($"contract/{guid}.pdf");
                var contractBlobResult = await contractBlob.UploadAsync(pdfStream);

                var signatureBlob = container.GetBlobClient($"signature/{guid}.jpg");
                var signatureBlobResult = await signatureBlob.UploadAsync(imageStream);

                var documentDetail = new DocumentDetailModel
                {
                    DocId = guid,
                    Name = fileUpload.Name,
                    Email = fileUpload.Email,
                    Location = fileUpload.Location,
                    ContractPath = contractBlob.Name,
                    SignaturePath = signatureBlob.Name,
                    SignedContractPath = signContractBlob.Name,
                    SignedDate = DateTime.UtcNow,
                };

                var tableResponse = await _tableService.Add(documentDetail);

                if (tableResponse)
                {
                    response = new AzureBlobResponse
                    {
                        Id = guid,
                        ContractBlobUri = contractBlob.Uri,
                        SignatureBlobUri = signatureBlob.Uri,
                        ContractFileName = contractBlob.Name,
                        SignatureFileName = signatureBlob.Name,
                        StatusCode = contractBlobResult.GetRawResponse().Status == 200 ? signatureBlobResult.GetRawResponse().Status : contractBlobResult.GetRawResponse().Status,
                        ReasonPhase = contractBlobResult.GetRawResponse().ReasonPhrase
                    };
                }
                else
                {
                    response.Id = guid;
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.ErrorMessage = "Table entity not added";
                }
            }
        }
        catch (RequestFailedException rfex)
        {
            response.IsError = true;
            response.StatusCode = rfex.Status;
            response.ErrorMessage = rfex.Message;
        }
        catch (Exception ex)
        {
            response.IsError = true;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ErrorMessage = ex.Message;
        }
        return response;
    }

}