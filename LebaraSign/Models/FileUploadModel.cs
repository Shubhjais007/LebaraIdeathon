namespace LebaraSign.Models
{
    public class FileUploadModel
    {
        public IFormFile ContractPdf { get; set; }
        public IFormFile SignatureImage { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
    }
}
