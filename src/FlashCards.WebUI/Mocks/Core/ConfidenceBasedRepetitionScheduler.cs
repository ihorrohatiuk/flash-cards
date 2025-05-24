using FlashCards.Core.Application.Dtos;

namespace FlashCards.WebUI.Mocks.Core;

public class ConfidenceBasedRepetitionScheduler
{
    private List<FlashCardDto> _flashCards;
    private Random _random;

    public ConfidenceBasedRepetitionScheduler(List<FlashCardDto> flashCards)
    {
        _random = new Random();
        _flashCards = flashCards;
    }

    public FlashCardDto GetNextFlashCard()
    {
        var weightedCards = new List<FlashCardDto>();

        foreach (var flashCard in _flashCards)
        {
            //TODO: Make wight a constant
            int weight = 6 - flashCard.Confidence;
            for (int i = 0; i < weight; i++)
            {
                weightedCards.Add(flashCard);
            }
        }
        
        int index = _random.Next(_flashCards.Count);
        return weightedCards[index];
    }

    public void UpdateConfidence(FlashCardDto flashCard, int confidence)
    {
        flashCard.Confidence = confidence;
    }
}