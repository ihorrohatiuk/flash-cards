using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Infrastructure.Persistence.DataModels;

public class CbrProgressEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [ForeignKey("FlashCardsUnit")]
    public Guid FlashCardsUnitId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public int Progress { get; set; }
    
    public FlashCardsUnitEntity? FlashCardsUnit { get; set; }
}