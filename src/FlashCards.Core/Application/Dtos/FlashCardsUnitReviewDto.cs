using FlashCards.Core.Domain.Entities;

namespace FlashCards.Core.Application.Dtos;

public class FlashCardsUnitReviewDto
{
    public FlashCardsUnit FlashCardsUnit { get; set; }
    public List<FlashCard> FlashCards { get; set; }
}