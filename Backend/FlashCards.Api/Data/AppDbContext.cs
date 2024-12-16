using System.IO;
using FlashCards.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FlashCards.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Progress> Progress { get; set; }
    public DbSet<FlashCard> FlashCard { get; set; }
    public DbSet<FlashCardsSet> FlashCardsSet { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}