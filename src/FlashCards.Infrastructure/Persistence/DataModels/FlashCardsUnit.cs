using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Infrastructure.Persistence.DataModels;

public class FlashCardsUnit
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name  { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Subject  { get; set; }
    
    [Required]
    public bool IsPrivate  { get; set; }
    
    [Required]
    [ForeignKey("User")]
    public Guid OwnerId { get; set; }
    
    [Required]
    public float Progress { get; set; }
    
    public User User { get; set; }
}
