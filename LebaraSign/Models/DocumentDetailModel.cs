namespace LebaraSign.Models
{
    public class DocumentDetailModel
    {
        public string DocId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public DateTime SignedDate { get; set; }
        public string ContractPath { get; set; }
        public string SignaturePath { get; set; }
        public string SignedContractPath { get; set; }
    }
}
