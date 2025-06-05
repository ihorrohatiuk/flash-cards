using FlashCards.Core.Application.Dtos;

namespace FlashCards.Core.Application.SpacedRepetition;

public class SM2Scheduler
{
    public void UpdateCard(Sm2FlashCardDto card, int quality)
    {
        if (quality < 0 || quality > 5)
            throw new ArgumentException("Quality rating must be between 0 and 5.");

        double ef = card.EasinessFactor;
        ef = ef + (0.1 - (5 - quality) * (0.08 + (5 - quality) * 0.02));
        if (ef < 1.3)
            ef = 1.3;
        
        int interval = 0;
        
        if (quality < 3)
        {
            card.Repetitions = 0;
            interval = 1;
        }
        else
        {
            card.Repetitions++;

            if (card.Repetitions == 1)
                interval = 1;
            else if (card.Repetitions == 2)
                interval = 6;
            else
                interval = (int)Math.Round(interval * ef);
        }

        card.EasinessFactor = ef;
        card.NextPracticeDate = DateTime.UtcNow.AddDays(interval);
    }
}