using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Core.Domain.Entities;

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
    public string? QuestionImagePath { get; set; }
    
    [Required]
    [MaxLength(300)]
    public string? Answer { get; set; }
    
    public FlashCardsUnit? FlashCardsUnit { get; set; }
}