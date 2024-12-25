using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Api.Data.Models;

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