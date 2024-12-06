using Azure;
using Azure.Data.Tables;

namespace LebaraSign.Models.Entity
{
    public class DocumentDetailEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string DocId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public DateTime SignedDate { get; set; } = DateTime.UtcNow;
        public string ContractPath { get; set; }
        public string SignaturePath { get; set; }
        public string SignedContractPath { get; set; }
    }
}
