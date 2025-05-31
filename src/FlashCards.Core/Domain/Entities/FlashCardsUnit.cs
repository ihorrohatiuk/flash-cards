using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Core.Domain.Entities;

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
}