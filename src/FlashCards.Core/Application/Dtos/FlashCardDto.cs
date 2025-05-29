using FlashCards.Core.Domain.Entities;

namespace FlashCards.Core.Application.Dtos;

public class FlashCardDto : FlashCard
{
    public int Confidence { get; set; } = 1;
}