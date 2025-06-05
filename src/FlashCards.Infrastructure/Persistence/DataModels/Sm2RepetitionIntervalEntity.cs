using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Infrastructure.Persistence.DataModels;

public class Sm2RepetitionIntervalEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid FlashCardId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid FlashCardsUnitId { get; set; }
    
    [Required]
    public int Repetitions { get; set; }
    
    [Required]
    public double Ef { get; set; }
    
    [Required]
    public DateTime NextRepetitionDate { get; set; }
}