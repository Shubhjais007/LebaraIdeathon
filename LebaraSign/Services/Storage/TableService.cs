using Azure;
using Azure.Data.Tables;
using LebaraSign.Models;
using LebaraSign.Models.Entity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace LebaraSign.Services.Storage
{
    public class TableService : ITableService
    {
        private readonly TableClient _tableClient;
        private readonly string partitionKey = "LebaraSign";
        public TableService(TableServiceClient tableServiceClient)
        {
            _tableClient = tableServiceClient.GetTableClient("LebaraSign");
        }
        public async Task<List<DocumentDetailModel>> GetAll()
        {
            var response = new List<DocumentDetailModel>();
            var tableResult = _tableClient.QueryAsync<DocumentDetailEntity>(filter: "", 100);
            await foreach (var item in tableResult)
            {
                //use Imapper later to modular code
                response.Add(new DocumentDetailModel
                {
                    DocId = item.DocId,
                    Name = item.Name,
                    Email = item.Email,
                    Location = item.Location,
                    SignedDate = item.SignedDate,
                    ContractPath = item.ContractPath,
                    SignaturePath = item.SignaturePath,
                    SignedContractPath = item.SignedContractPath     
                });
            }

            return response;
        }

        public async Task<DocumentDetailModel> GetById(string id)
        {
            //var response = new DocumentDetailModel();
            var tableResult = await _tableClient.GetEntityAsync<DocumentDetailEntity>(partitionKey, id);
            var response = new DocumentDetailModel
            {
                DocId = tableResult.Value.DocId,
                Name = tableResult.Value.Name,
                Email = tableResult.Value.Email,
                Location = tableResult.Value.Location,
                SignedDate = tableResult.Value.SignedDate,
                ContractPath = tableResult.Value.ContractPath,
                SignaturePath = tableResult.Value.SignaturePath,
                SignedContractPath = tableResult.Value.SignedContractPath
            };
            return response;
        }

        public async Task<bool> Add(DocumentDetailModel request)
        {
            try
            {
                var documentEntity = new DocumentDetailEntity
                {
                    PartitionKey = partitionKey,
                    RowKey = request.DocId,
                    DocId = request.DocId,
                    Name = request.Name,
                    Email = request.Email,
                    Location = request.Location,
                    SignedDate = request.SignedDate,
                    ContractPath = request.ContractPath,
                    SignaturePath = request.SignaturePath,
                    SignedContractPath = request.SignedContractPath
                };

                var response = await _tableClient.AddEntityAsync<DocumentDetailEntity>(documentEntity);
                return true;
            }
            catch (RequestFailedException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Update(string id, DocumentDetailModel request)
        {
            try
            {
                var currentDocumentDetail = await _tableClient.GetEntityAsync<DocumentDetailEntity>(partitionKey, id);
                if (currentDocumentDetail == null)
                {
                    return false;
                }
                currentDocumentDetail.Value.DocId = request.DocId;
                currentDocumentDetail.Value.Name = request.Name;
                currentDocumentDetail.Value.Email = request.Email;
                currentDocumentDetail.Value.Location = request.Location;
                currentDocumentDetail.Value.SignedDate = request.SignedDate;
                currentDocumentDetail.Value.ContractPath = request.ContractPath;
                currentDocumentDetail.Value.SignaturePath = request.SignaturePath;
                currentDocumentDetail.Value.SignedContractPath = request.SignedContractPath;

                await _tableClient.UpdateEntityAsync<DocumentDetailEntity>(currentDocumentDetail, currentDocumentDetail.Value.ETag);
                return true;
            }
            catch (RequestFailedException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteById(string id)
        {
            try
            {
                var documentDetail = await _tableClient.GetEntityAsync<DocumentDetailEntity>(partitionKey, id);
                await _tableClient.DeleteEntityAsync(documentDetail.Value.PartitionKey, documentDetail.Value.RowKey);
                return true;
            }
            catch (RequestFailedException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        } 
    }
}
