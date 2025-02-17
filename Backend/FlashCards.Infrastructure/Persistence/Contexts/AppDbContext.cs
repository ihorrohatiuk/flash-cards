using FlashCards.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Infrastructure.Persistence.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<FlashCard> FlashCards { get; set; }
    public DbSet<FlashCardsUnit> FlashCardsUnits { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}