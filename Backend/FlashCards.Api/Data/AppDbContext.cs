using FlashCards.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<FlashCard> FlashCards { get; set; }
    public DbSet<FlashCardsUnit> FlashCardsUnits { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}