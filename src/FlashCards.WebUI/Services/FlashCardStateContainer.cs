using FlashCards.Core.Domain.Entities;

namespace FlashCards.WebUI.Services;

public class FlashCardStateContainer
{
    public List<FlashCard>? FlashCards { get; set; }
}