using System.ComponentModel.DataAnnotations;
using FlashCards.Core.Domain.Constants;

namespace FlashCards.Core.Domain.Entities;

public class FlashCard
{
    [Required]
    [MaxLength(AppConstants.MaxQuestionLength)]
    public string Question { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(AppConstants.MaxAnswerLength)]
    public string Answer { get; set; } = string.Empty;
}