using System.Text;

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

    public static string GetFirstLettersOfName(string fullName)
    {
        string[] names = fullName.Split(" ");
        var sb = new StringBuilder();
        // Returning only 2 letters, even if name has more then 2 words
        for (int i = 0; i < 2; i++)
        {
            sb.Append(names[i][0].ToString().ToUpper());
        }
        
        return sb.ToString();
    }
}
