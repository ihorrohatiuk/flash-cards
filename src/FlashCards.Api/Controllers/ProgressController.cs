using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Core.Application.Dtos;
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
public class ProgressController : ControllerBase
{
    private AppDbContext _context;

    public ProgressController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("cbr/{flashCardsUnitId}")]
    public async Task<IActionResult> WriteProgressForUnit(Guid flashCardsUnitId, [FromBody] int progress)
    {
        _ = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        Guid userId = Guid.Parse(User.FindFirst(JwtClaims.UserId)?.Value);
        
        CbrProgressEntity? cbrProgress = await _context.CbrProgresses
            .Where(p => p.UserId == userId)
            .FirstOrDefaultAsync(p => flashCardsUnitId == p.FlashCardsUnitId);

        if (cbrProgress == null)
        {
            _context.CbrProgresses.Add(new CbrProgressEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FlashCardsUnitId = flashCardsUnitId,
                Progress = progress,
            });
            
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Cbr progress has been saved", cbrProgress = cbrProgress });
        }
        
        cbrProgress.Progress = progress;
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Cbr progress has been saved", cbrProgress = cbrProgress });
    }
    
    [HttpGet("cbr/{flashCardsUnitId}")]
    public IActionResult GetProgressByUnitId(Guid flashCardsUnitId)
    {
        _ = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        Guid userId = Guid.Parse(User.FindFirst(JwtClaims.UserId)?.Value);
        
        CbrProgressEntity? cbrProgress = _context.CbrProgresses
            .Where(p => p.UserId == userId)
            .FirstOrDefault(p => flashCardsUnitId == p.FlashCardsUnitId);
        
        if (cbrProgress == null)
            return NotFound();
        
        return Ok(cbrProgress);
    }
    
    [HttpGet("sm2/{flashCardsUnitId}")]
    public async Task<IActionResult> GetSm2FlashCardsByUnit(Guid flashCardsUnitId)
    {
        Guid userId = Guid.Parse(User.FindFirst(JwtClaims.UserId)?.Value);

        var _flashCardsSm2 = await _context.Sm2RepetitionIntervals
            .Where(f => f.UserId == userId && f.FlashCardsUnitId == flashCardsUnitId)
            .ToListAsync();

        if (_flashCardsSm2.Count.Equals(0))
        {
            var unitFlashCards = await _context.FlashCards
                .Where(f => f.FlashCardsUnitId == flashCardsUnitId)
                .ToListAsync();

            if (unitFlashCards.Count.Equals(0))
                return NotFound("No flash cards found in this unit.");

            var newSm2Entries = unitFlashCards.Select(flashCard => new Sm2RepetitionIntervalEntity
            {
                Id = Guid.NewGuid(),
                FlashCardId = flashCard.Id,
                UserId = userId,
                FlashCardsUnitId = flashCardsUnitId,
                Repetitions = 0,
                Ef = 2.5,
                NextRepetitionDate = DateTime.UtcNow
            }).ToList();

            _context.Sm2RepetitionIntervals.AddRange(newSm2Entries);
            await _context.SaveChangesAsync();

            _flashCardsSm2 = newSm2Entries;
        }

        _flashCardsSm2 = _flashCardsSm2
            .Where(f => f.NextRepetitionDate <= DateTime.UtcNow)
            .ToList();

        if (_flashCardsSm2.Count == 0)
            return NotFound("User doesn't have flash cards for today's session");

        var flashCardIds = _flashCardsSm2.Select(f => f.FlashCardId).ToList();
        var flashCards = await _context.FlashCards
            .Where(f => flashCardIds.Contains(f.Id))
            .ToListAsync();

        var _cardDtos = _flashCardsSm2.Select(flashCardSm2 =>
        {
            var card = flashCards.FirstOrDefault(c => c.Id == flashCardSm2.FlashCardId);
            return new Sm2FlashCardDto
            {
                Id = flashCardSm2.FlashCardId,
                Question = card?.Question,
                Answer = card?.Answer,
                EasinessFactor = flashCardSm2.Ef,
                Repetitions = flashCardSm2.Repetitions,
                NextPracticeDate = flashCardSm2.NextRepetitionDate
            };
        }).ToList();

        return Ok(_cardDtos);
    }
    
    [HttpPost("sm2/{flashCardsUnitId}/update")]
    public async Task<IActionResult> UpdateSm2CardRepetitionDate(Guid flashCardsUnitId, [FromBody] Sm2FlashCardDto flashCardDto)
    {
        Guid userId = Guid.Parse(User.FindFirst(JwtClaims.UserId)?.Value);

        var sm2Card = await _context.Sm2RepetitionIntervals
            .FirstOrDefaultAsync(f => f.UserId == userId 
                                      && f.FlashCardsUnitId == flashCardsUnitId 
                                      && f.FlashCardId == flashCardDto.Id);

        if (sm2Card == null)
            return NotFound("Flash card not found for user in this unit");

        sm2Card.Repetitions = flashCardDto.Repetitions;
        sm2Card.Ef = flashCardDto.EasinessFactor;
        sm2Card.NextRepetitionDate = flashCardDto.NextPracticeDate;

        _context.Sm2RepetitionIntervals.Update(sm2Card);
        await _context.SaveChangesAsync();

        return Ok("Flash card repetition data updated successfully");
    }
}