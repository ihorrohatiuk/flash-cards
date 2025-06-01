using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FlashCards.Core.Application.Dtos;
using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.Contexts;
using FlashCards.Infrastructure.Persistence.DataModels;
using FlashCards.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UnitsController : ControllerBase
{
    private AppDbContext _context;

    public UnitsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost("add-unit")]
    public async Task<IActionResult> AddUnit([FromBody] FlashCardsUnitDto? flashCardsUnitDto)
    {
        if (flashCardsUnitDto == null)
            return BadRequest("Invalid request payload");
        
        //Getting token from request
        _ = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        Guid userId = Guid.Parse(User.FindFirst(JwtClaims.UserId)?.Value);
        
        Guid unitId = Guid.NewGuid();
        _context.FlashCardsUnits.Add(new FlashCardsUnitEntity
        {
            Id = unitId,
            Name = flashCardsUnitDto.FlashCardsUnit.Name,
            Subject = flashCardsUnitDto.FlashCardsUnit.Subject,
            OwnerId = userId
        });

        foreach (var card in flashCardsUnitDto.FlashCards)
        {
            var flashCardEntity = new FlashCardEntity
            {
                Id = Guid.NewGuid(),
                Question = card.Question,
                Answer = card.Answer,
                FlashCardsUnitId = unitId,
            };
            _context.FlashCards.Add(flashCardEntity);
        }

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Unit and flashcards added successfully", UnitId = unitId });
    }
}