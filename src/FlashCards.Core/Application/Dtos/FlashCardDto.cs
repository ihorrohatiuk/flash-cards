namespace FlashCards.Core.Application.Dtos;

public class FlashCardDto
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int Confidence { get; set; } = 1;
}