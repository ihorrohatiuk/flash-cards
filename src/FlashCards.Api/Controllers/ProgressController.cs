using System;
using System.Linq;
using System.Threading.Tasks;
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
        
        CbrProgress? cbrProgress = await _context.CbrProgresses
            .Where(p => p.UserId == userId)
            .FirstOrDefaultAsync(p => flashCardsUnitId == p.FlashCardsUnitId);

        if (cbrProgress == null)
        {
            _context.CbrProgresses.Add(new CbrProgress
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
        
        CbrProgress? cbrProgress = _context.CbrProgresses
            .Where(p => p.UserId == userId)
            .FirstOrDefault(p => flashCardsUnitId == p.FlashCardsUnitId);
        
        if (cbrProgress == null)
            return NotFound();
        
        return Ok(cbrProgress);
    }
}