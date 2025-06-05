using FlashCards.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Infrastructure.Persistence.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<FlashCardEntity> FlashCards { get; set; }
    public DbSet<FlashCardsUnitEntity> FlashCardsUnits { get; set; }
    public DbSet<CbrProgressEntity> CbrProgresses { get; set; }
    public DbSet<Sm2RepetitionIntervalEntity> Sm2RepetitionIntervals { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}