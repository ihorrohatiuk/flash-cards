namespace FlashCards.Infrastructure.Helpers;

public static class TextHelper
{
    public static string TruncateText(string text, int maxLength = 20)
    {
        if (string.IsNullOrEmpty(text))
        {
            return String.Empty;
        }
        return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
    }
}
