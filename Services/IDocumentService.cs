using LebaraSign.Models;

namespace LebaraSign.Services
{
    public interface IDocumentService
    {
        //Task<AzureBlobResponse> UploadFileAsync(string name, string email, string location, Stream contractFileStream, Stream imageFileStream);
        Task<AzureBlobResponse> UploadFileAsync(FileUploadModel fileUpload);
    }
}
