using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Infrastructure.Persistence.DataModels;

public class FlashCard
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [ForeignKey("FlashCardsUnit")]
    public Guid FlashCardsUnitId { get; set; }
    
    [Required]
    [MaxLength(300)]
    public string? Question { get; set; }
    
    [Required]
    [MaxLength(300)]
    public string? Answer { get; set; }
    
    [Required]
    public int Confidence { get; set; }
    
    public FlashCardsUnit? FlashCardsUnit { get; set; }
}