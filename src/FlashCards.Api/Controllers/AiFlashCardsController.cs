using System;
using System.IO;
using System.Threading.Tasks;
using FlashCards.Core.Domain.Constants;
using FlashCards.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiFlashCardsController : ControllerBase
{
    private readonly AiFlashCardsService _aiFlashCardsService;
    
    public AiFlashCardsController(AiFlashCardsService service)
    {
        _aiFlashCardsService = service;    
    }

    [HttpPost("CreateFlashCards")]
    public async Task<IActionResult> CreateFlashCards([FromForm] IFormFile? file, [FromForm] int numberOfCards)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is required.");
        }
        if (file.Length > AppConstants.MaximumFileSizeKb * 1024)
        {
            return BadRequest($"File size must be less than or equal to {AppConstants.MaximumFileSizeKb} KB.");
        }

        var fileExtension = Path.GetExtension(file.FileName);
        var tempFilePath = Path.ChangeExtension(Path.GetRandomFileName(), fileExtension);
        await using (var stream = System.IO.File.Create(tempFilePath))
        {
            await file.CopyToAsync(stream);
        }

        try
        {
            var flashCards = await _aiFlashCardsService.CreateFlashCardsFromFileUsingAi(tempFilePath, numberOfCards);
            return Ok(flashCards);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, $"An error occurred while generating flash cards, please try again later.");
        }
        finally
        {
            System.IO.File.Delete(tempFilePath);
        }
    }
}