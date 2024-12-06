using LebaraSign.Models;

namespace LebaraSign.Services
{
    public interface IBlobService
    {
        Task<AzureBlobResponse> UploadFileAsync(string name, string email, string location, Stream contractFileStream, Stream imageFileStream);
    }
}
