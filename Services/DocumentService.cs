using Azure;
using Azure.Storage.Blobs;
using LebaraSign.Models;
using LebaraSign.Services.Storage;
using System.Net;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using static PdfSharp.Pdf.PdfDictionary;
using PdfSharp.Pdf;
using PdfDocument = UglyToad.PdfPig.PdfDocument;
using UglyToad.PdfPig.Content;
using System.ComponentModel;
using iText.Kernel.Pdf;
using iText.Signatures;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
//using System.Security.Cryptography.X509Certificates;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.IsisMtt.Ocsp;
using System.Security.Cryptography.X509Certificates;
using iText.Commons.Bouncycastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Crypto;


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
                #region Digital Sign

                //X509Certificate2 cert = new X509Certificate2("Path_Add_for_Certificate", "Mahadev@321#");
                //var reader = new iText.Kernel.Pdf.PdfReader(modifiedPdfStream);
                //PdfWriter writer = new PdfWriter(signedPdfStream);
                //var pdfDocument = new iText.Kernel.Pdf.PdfDocument(reader, writer);

                //var signer = new PdfSigner(reader, writer, new StampingProperties());
                //var pk = Org.BouncyCastle.Security.DotNetUtilities.GetKeyPair(cert.PrivateKey).Private;
                //var chain = new[] { Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(cert) };
                //var path = Directory.GetCurrentDirectory();
                //IExternalSignature pks = new PrivateKeySignature(pk, "SHA-256");
                //signer.SignDetached(pks, chain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);               

                //X509Certificate2 cert = new X509Certificate2(path, certPassword, X509KeyStorageFlags.Exportable);

                //// Convert X509Certificate2 to BouncyCastle objects
                //Pkcs12Store pkcs12Store;
                ////using (var certStream = new MemoryStream(cert.Export(X509ContentType.Pfx)))
                //using (FileStream certStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                //{
                //    Pkcs12StoreBuilder builder = new Pkcs12StoreBuilder();
                //    pkcs12Store = builder.Build();
                //    pkcs12Store.Load(certStream, certPassword.ToCharArray());
                //}

                //// Extract the private key and certificate chain
                //string alias = null;
                //foreach (string currentAlias in pkcs12Store.Aliases)
                //{
                //    if (pkcs12Store.IsKeyEntry(currentAlias))
                //    {
                //        alias = currentAlias;
                //        break;
                //    }
                //}
                ////AsymmetricKeyParameter privateKey = pkcs12Store.GetKey(alias).Key;
                ////X509CertificateEntry[] chain = pkcs12Store.GetCertificateChain(alias);
                ////Org.BouncyCastle.X509.X509Certificate[] bouncyCastleChain = Array.ConvertAll(chain, entry => entry.Certificate);

                //AsymmetricKeyParameter privateKey = pkcs12Store.GetKey(alias).Key;
                //Org.BouncyCastle.X509.X509Certificate[] bouncyCastleChain = Array.ConvertAll(pkcs12Store.GetCertificateChain(alias), entry => entry.Certificate);

                //// Convert BouncyCastle AsymmetricKeyParameter to iText IPrivateKey
                ////byte[] privateKeyBytes = GetPrivateKeyBytes(privateKey);
                ////IPrivateKey iTextPrivateKey = (IPrivateKey)PrivateKeyFactory.CreateKey(privateKeyBytes);

                //// Convert the BouncyCastle certificates to iText's IX509Certificate
                ////IX509Certificate[] iTextCertificates = ConvertToITextCertificates(bouncyCastleChain);

                //// iText7 signing
                //var reader = new iText.Kernel.Pdf.PdfReader(modifiedPdfStream);
                ////var writer = new iText.Kernel.Pdf.PdfWriter(signedPdfStream);
                //PdfSigner signer = new PdfSigner(reader, signedPdfStream, new StampingProperties());

                ////IExternalSignature pks = new PrivateKeySignature((IPrivateKey)privateKey, DigestAlgorithms.SHA256);

                //// Use the RSASSAPkcs1Signature class, compatible with BouncyCastle private keys
                //IExternalSignature pks = new PrivateKeySignature(  , DigestAlgorithms.SHA256);
                //signer.SignDetached(pks, bouncyCastleChain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);

                #endregion Digital Sign

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

    private byte[] GetPrivateKeyBytes(AsymmetricKeyParameter privateKey)
    {
        // Extract the private key bytes for both RSA and EC keys
        if (privateKey is RsaKeyParameters rsaKey)
        {
            return rsaKey.Modulus.ToByteArrayUnsigned();  // Convert RSA private key to byte array
        }
        else if (privateKey is ECPrivateKeyParameters ecKey)
        {
            return ecKey.D.ToByteArray();  // Convert EC private key to byte array
        }

        throw new NotSupportedException("Unsupported private key type");
    }
   

    //private async Task UploadToBlobStorage(Stream pdfStream, string blobName)
    //{
    //    BlobServiceClient blobServiceClient = new BlobServiceClient(blobConnectionString);
    //    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
    //    await containerClient.CreateIfNotExistsAsync();
    //    BlobClient blobClient = containerClient.GetBlobClient(blobName);
    //    await blobClient.UploadAsync(pdfStream, overwrite: true);
    //}

    //private byte[] ConvertToByteArray(Stream inputStream)
    //{
    //    using (var memoryStream = new MemoryStream())
    //    {
    //        inputStream.CopyTo(memoryStream);
    //        return memoryStream.ToArray();
    //    }
    //}
}