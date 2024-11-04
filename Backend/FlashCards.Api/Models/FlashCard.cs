namespace FlashCards.Api.Models;

public class FlashCard
{
    public int Id { get; set; }
    public int FlashCardsSetId { get; set; }
    public string? Question { get; set; }
    public string? QuestionImagePath { get; set; }
    public string? Answer { get; set; }

    public FlashCard(int id, int flashCardsSetId, string? question, string? questionImagePath, string? answer)
    {
        Id = id;
        FlashCardsSetId = flashCardsSetId;
        Question = question;
        QuestionImagePath = questionImagePath;
        Answer = answer;
    }
    
    public override string ToString()
    {
        return $"{{\n\tId: {Id}," +
               $"\n\tFlashCardsSetId: {FlashCardsSetId}," +
               $"\n\tQuestion: {Question}," +
               $"\n\tQuestionImagePath: {QuestionImagePath}," +
               $"\n\tAnswer: {Answer}\n}}";
    }

}