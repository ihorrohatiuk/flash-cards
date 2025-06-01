using FlashCards.Core.Domain.Entities;

namespace FlashCards.Core.Application.Dtos;

public class CbrFlashCardDto : FlashCard
{
    public int Confidence { get; set; } = 1;
}