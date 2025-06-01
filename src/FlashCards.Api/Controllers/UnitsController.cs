using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FlashCards.Core.Application.Dtos;
using FlashCards.Core.Domain.Constants;
using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.Contexts;
using FlashCards.Infrastructure.Persistence.DataModels;
using FlashCards.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    
    [HttpGet("{unitId}")]
    public async Task<IActionResult> GetUnitById(Guid unitId)
    {
        var flashCardsUnit = await _context.FlashCardsUnits
            .FirstOrDefaultAsync(u => u.Id == unitId);

        if (flashCardsUnit == null)
            return NotFound("FlashCardsUnit not found.");

        var flashCards = await _context.FlashCards
            .Where(fc => fc.FlashCardsUnitId == unitId)  
            .ToListAsync();
        
        var flashCardsDomain = flashCards.Select(fcEntity => new FlashCard
        {
            Question = fcEntity.Question,
            Answer = fcEntity.Answer
        }).ToList();

        var result = new FlashCardsUnitDto
        {
            FlashCardsUnit = flashCardsUnit,
            FlashCards = flashCardsDomain
        };

        return Ok(result);
    }
    
    [HttpGet("GetByOwner/{ownerId}")]
    public async Task<IActionResult> GetUnitHeadersByOwner(Guid ownerId)
    {
        var units = await _context.FlashCardsUnits
            .Where(u => u.OwnerId == ownerId)
            .ToListAsync();

        if (!units.Any())
            return NotFound("No units found for this owner.");

        var result = new List<FlashCardsUnitInfoDto>();

        foreach (var unit in units)
        {
            var cards = await _context.FlashCards
                .Where(c => c.FlashCardsUnitId == unit.Id)
                .ToListAsync();

            int cardsQuantity = cards.Count;
            
            // TODO: Add progress count here
            var ownerName = await _context.Users
                .Where(u => u.Id == ownerId)
                .Select(u => u.FirstName + " " + u.LastName)
                .FirstOrDefaultAsync() ?? "No Name";

            result.Add(new FlashCardsUnitInfoDto
            {
                Id = unit.Id,
                Name = unit.Name,
                Subject = unit.Subject,
                OwnerId = unit.OwnerId,
                Owner = ownerName,
                CardsQuantity = cardsQuantity,
                Progress = 57
            });
        }

        return Ok(result);
    }
    
    [HttpPost("delete-unit")]
    public async Task<IActionResult> DeleteUnit([FromQuery] Guid flashCardsUnitId)
    {
        // TODO: ALSO DELETE ALL FLASHCARDS in PROGRESS, SRS METHOD, THAT are linked to this unit!!! 

        //Getting token from request
        _ = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        Guid userId = Guid.Parse(User.FindFirst(JwtClaims.UserId)?.Value);
        string userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        
        if (flashCardsUnitId == Guid.Empty)
            return BadRequest("Invalid unit id.");

        var unit = await _context.FlashCardsUnits.FindAsync(flashCardsUnitId);
        if (unit == null)
            return NotFound("Unit not found.");
        if (unit.OwnerId != userId && !string.Equals(userRole, RolesType.Admin))
            return Forbid();

        var cards = _context.FlashCards
            .Where(c => c.FlashCardsUnitId == flashCardsUnitId);

        // Remove flashcards
        _context.FlashCards.RemoveRange(cards);
        
        // Remove unit
        _context.FlashCardsUnits.Remove(unit);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Unit deleted successfully." });
    }
}