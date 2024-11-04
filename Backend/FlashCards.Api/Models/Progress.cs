namespace FlashCards.Api.Models;

public class Progress
{
    public int Id { get; set; }
    public int FlashCardsSetId { get; set; }
    public int UserId { get; set; }

    public Progress(int id, int flashCardsSetId, int userId)
    {
        Id = id;
        FlashCardsSetId = flashCardsSetId;
        UserId = userId;
    }
    
    public override string ToString()
    {
        return $"{{\n\tId: {Id}," +
               $"\n\tFlashCardsSetId: {FlashCardsSetId}," +
               $"\n\tUserId: {UserId}\n}}";
    }
}