using LebaraSign.Models;
using System.Diagnostics.Contracts;

namespace LebaraSign.Services
{
    public interface IDocumentService
    {
        Task<AzureBlobResponse> UploadFileAsync(FileUploadModel fileUpload);
        Task<DownloadResponse> DownloadContract(string contractId);
    }
}
