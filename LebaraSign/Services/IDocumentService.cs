using LebaraSign.Models;

namespace LebaraSign.Services
{
    public interface IDocumentService
    {
        Task<AzureBlobResponse> UploadFileAsync(FileUploadModel fileUpload);
    }
}
