namespace LebaraSign.Models
{
    public class AzureBlobResponse
    { 
        public int StatusCode { get; set; }
        public string? Id { get; set; }
        public string ReasonPhase { get; set; }
        public Uri ContractBlobUri { get; set; }
        public Uri SignatureBlobUri { get; set; }
        public bool IsError { get; set; }
        public string ContractFileName { get; set; }
        public string SignatureFileName { get; set; }
        public string ErrorMessage { get; set; }

    }
}
