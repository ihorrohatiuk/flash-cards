using FlashCards.Core.Domain.Entities;

namespace FlashCards.Core.Application.Dtos;

public class FlashCardsUnitInfoDto : FlashCardsUnit
{
    public string Owner { get; set; } = string.Empty;
    public float Progress { get; set; }
    public int CardsQuantity { get; set; }
}