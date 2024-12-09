using LebaraSign.Common;
using LebaraSign.Models;
using LebaraSign.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LebaraSign.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService ;
    private readonly long pdfMaxLength = 3*1024*1024;
    private readonly long imageMaxLength = 600*1024;
    public DocumentController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("upload"), DisableRequestSizeLimit]
    public async Task<IActionResult> UploadAsync([FromForm] FileUploadModel fileUploadModel)
    {
        if (string.IsNullOrEmpty(fileUploadModel.ContractPdf.ContentType) || !fileUploadModel.ContractPdf.ContentType.Equals("application/pdf", StringComparison.InvariantCultureIgnoreCase) || pdfMaxLength <= fileUploadModel.ContractPdf.Length)
        {
            return BadRequest("Contract File format/Type is not correct or File Size is more than 3 MB.");
        }

        if (string.IsNullOrEmpty(fileUploadModel.SignatureImage.ContentType) || !fileUploadModel.SignatureImage.ContentType.Equals("image/jpeg", StringComparison.InvariantCultureIgnoreCase) || imageMaxLength <= fileUploadModel.SignatureImage.Length)
        {
            return BadRequest("Signature Image format/Type is not correct or File Size is more than 3 MB.");
        }
        
        var result = await _documentService.UploadFileAsync(fileUploadModel);
        if (result.IsError)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("[action]/{contractId}")]
    public async Task<IActionResult> DownloadContract(string contractId)
    {
        byte[] rawBytes = Array.Empty<byte>();
        if (string.IsNullOrEmpty(contractId))
        {
            return BadRequest("Contract ID is required.");
        }
        var contractResponse = await _documentService.DownloadContract(contractId);
        if (contractResponse.SignedContractLink == null)
        {
            return BadRequest(contractResponse);
        }
        return Ok(contractResponse);
    }
}
