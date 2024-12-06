using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LebaraSign.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Threading.Tasks;

namespace LebaraSign.Services;

public class BlobService : IBlobService
{
   // private readonly BlobContainerClient _blobContainerClient;
    private readonly BlobServiceClient _blobServiceClient;

    //public BlobService(string connectionString, string containerName, BlobServiceClient blobServiceClient)
    //{
    //    //var blobServiceClient = new BlobServiceClient(connectionString);
    //    // _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
    //    _blobServiceClient = blobServiceClient;
    //}

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }


    public async Task<AzureBlobResponse> UploadFileAsync(string name, string email, string location, Stream contractFileStream, Stream imageFileStream)
    {
        var response = new AzureBlobResponse();
        //
        try
        {
            var guid = Guid.NewGuid().ToString();
            var container = _blobServiceClient.GetBlobContainerClient("lebarasign");
            await container.CreateIfNotExistsAsync();

            var contractBlob = container.GetBlobClient($"contract/{guid}.pdf");
            var contractBlobResult = await contractBlob.UploadAsync(contractFileStream);

            var signatureBlob = container.GetBlobClient($"signature/{guid}.pdf");
            var signatureBlobResult = await signatureBlob.UploadAsync(imageFileStream);
            response = new AzureBlobResponse
            {
                Id = guid,
                ContractBlobUri = contractBlob.Uri,
                SignatureBlobUri = signatureBlob.Uri,
                ContractFileName = contractBlob.Name,
                SignatureFileName = signatureBlob.Name,
                StatusCode = contractBlobResult.GetRawResponse().Status,
                ReasonPhase = contractBlobResult.GetRawResponse().ReasonPhrase
            };
        }
        catch(RequestFailedException rfex) 
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



    //// Create
    //public async Task<string> UploadFileAsync(Stream file)
    //{
    //    try
    //    {
    //        var blobClient = _blobContainerClient.GetBlobClient("myFile.pdf");
    //        await blobClient.UploadAsync(file);
    //        return blobClient.Uri.ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        return string.Empty;
    //    }
    //}

    //// Read
    //public async Task<Stream> DownloadFileAsync(string fileName)
    //{
    //    var blobClient = _blobContainerClient.GetBlobClient(fileName);
    //    var blobDownloadInfo = await blobClient.DownloadAsync();
    //    return blobDownloadInfo.Value.Content;
    //}

    ////// Update
    ////public async Task<string> UpdateFileAsync(Stream file, string existingFileName)
    ////{
    ////    // Delete the existing file
    ////    await DeleteFileAsync(existingFileName);

    ////    // Upload the new file
    ////    return await UploadFileAsync(file);
    ////}

    //// Delete
    //public async Task DeleteFileAsync(string fileName)
    //{
    //    var blobClient = _blobContainerClient.GetBlobClient(fileName);
    //    await blobClient.DeleteIfExistsAsync();
    //}

    //// List Blobs
    //public async Task<IEnumerable<string>> ListFilesAsync()
    //{
    //    var blobs = new List<string>();
    //    await foreach (var blobItem in _blobContainerClient.GetBlobsAsync())
    //    {
    //        blobs.Add(blobItem.Name);
    //    }
    //    return blobs;
    //}   
}

