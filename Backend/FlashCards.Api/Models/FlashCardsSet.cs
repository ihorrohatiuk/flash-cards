using System;

namespace FlashCards.Api.Models;

public class FlashCardsSet
{
    public int Id { get; set; }
    public string? Theme  { get; set; }
    public int UserId { get; set; }
    public DateTime ModifiedDate { get; set; }

    public FlashCardsSet(int id, string? theme, int userId, DateTime modifiedDate)
    {
        Id = id;
        Theme = theme;
        UserId = userId;
        ModifiedDate = modifiedDate;
    }
    
    public override string ToString()
    {
        return $"{{\n\tId: {Id}," +
               $"\n\tTheme: {Theme}," +
               $"\n\tUserId: {UserId}," +
               $"\n\tModifiedDate: {ModifiedDate:yyyy-MM-dd HH:mm:ss}\n}}";
    }
}