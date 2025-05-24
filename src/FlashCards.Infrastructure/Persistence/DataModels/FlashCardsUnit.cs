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
    public string Theme  { get; set; }
    
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    [Required]
    public DateTime ModifiedDate { get; set; }
    
    public User User { get; set; }
}
