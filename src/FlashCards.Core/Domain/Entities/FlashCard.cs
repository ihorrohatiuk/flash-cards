using System.ComponentModel.DataAnnotations;

namespace FlashCards.Core.Domain.Entities;

public class FlashCard
{
    [Required]
    [MaxLength(300)]
    public string Question { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(300)]
    public string Answer { get; set; } = string.Empty;
}