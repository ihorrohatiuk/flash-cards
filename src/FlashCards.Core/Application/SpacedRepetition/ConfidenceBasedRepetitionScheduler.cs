using FlashCards.Core.Application.Dtos;
using FlashCards.Core.Domain.Constants;

namespace FlashCards.Core.Application.SpacedRepetition;

public class ConfidenceBasedRepetitionScheduler
{
    private List<CbrFlashCardDto> _flashCards;
    private Random _random;

    public ConfidenceBasedRepetitionScheduler(List<CbrFlashCardDto> flashCards)
    {
        _random = new Random();
        _flashCards = flashCards;
    }

    public CbrFlashCardDto GetNextFlashCard()
    {
        var weightedCards = new List<CbrFlashCardDto>();

        foreach (var flashCard in _flashCards)
        {
            int weight = AlgorithmConstants.ConfidenceBasedRepetitionConstant - flashCard.Confidence;
            for (int i = 0; i < weight; i++)
            {
                weightedCards.Add(flashCard);
            }
        }
        
        int index = _random.Next(weightedCards.Count);
        return weightedCards[index];
    }

    public void UpdateConfidence(CbrFlashCardDto cbrFlashCard, int confidence)
    {
        cbrFlashCard.Confidence = confidence;
    }
}