using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlashCards.Core.Domain.Entities;

namespace FlashCards.Infrastructure.Persistence.DataModels;

public class FlashCardEntity : FlashCard
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [ForeignKey("FlashCardsUnit")]
    public Guid FlashCardsUnitId { get; set; }
    
    public FlashCardsUnitEntity? FlashCardsUnit { get; set; }
}