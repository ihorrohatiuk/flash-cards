namespace FlashCards.WebUI.Mocks;

public class FlashCardsUnitDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    public string Owner { get; set; } = string.Empty;
    public float Progress { get; set; }
    public int CardsQuantity { get; set; }
}