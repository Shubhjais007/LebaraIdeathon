using LebaraSign.Models;
using System.Reflection.Metadata;

namespace LebaraSign.Services.Storage
{
    public interface ITableService
    {
        Task<List<DocumentDetailModel>> GetAll();

        Task<DocumentDetailModel> GetById(string id);

        Task<bool> Add(DocumentDetailModel model);
        Task<bool> Update(string id, DocumentDetailModel model);
        Task<bool> DeleteById(string id);

    }
}
