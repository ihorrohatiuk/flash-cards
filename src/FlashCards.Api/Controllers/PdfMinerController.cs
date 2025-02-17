using System.IO;
using System.Threading.Tasks;
using FlashCards.Core.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PdfMinerController : ControllerBase
{
    private LLMService _llmService;
    
    [HttpPost]
    public async Task<ActionResult> GetFlashCardsFromPdfFile([FromForm] IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded or the file is empty.");
        
        if (file.ContentType != "application/pdf")
            return BadRequest("Only PDF files are allowed.");
        
        _llmService = new LLMService();
        try
        {
            var tempPath = "C:\\Users\\rogat\\PERSONAL\\Bachaleurs work\\PdfExamples\\";
            var filePath = Path.Combine(tempPath, file.FileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            var fileContent = _llmService.ParsPdfFile(filePath);
            var result = _llmService.MakeFlashCardsFromPdfContent(fileContent);

            return Ok(result.Result);
        }
        catch
        {
            return StatusCode(500, "An error occurred while uploading the file.");
        }
    }
}