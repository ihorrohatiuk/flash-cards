using FlashCards.Core.Domain.Entities;

namespace FlashCards.Core.Application.Dtos;

public class Sm2FlashCardDto : FlashCard
{
    public Guid Id { get; set; }
    public double EasinessFactor { get; set; } = 1.3;
    public int Repetitions { get; set; } = 0;
    public DateTime NextPracticeDate { get; set; }
}