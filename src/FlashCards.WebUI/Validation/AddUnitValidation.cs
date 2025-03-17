namespace FlashCards.WebUI.Validation;

public static class AddUnitValidation
{
    public static IEnumerable<string> UnitNameValidation(string unitName)
    {
        if (string.IsNullOrWhiteSpace(unitName) || string.IsNullOrEmpty(unitName))
        {
            yield return "Unit name cannot be empty.";
            yield break;
        }
        
        // TODO: define unitName maximum length as a constant in domain layer
        if (unitName.Length is < 2 or > 50)
        {
            yield return $"Unit name must be between 2 and 100 characters long.";
        }
    }

    public static IEnumerable<string> QuestionValidation(string question)
    {
        if (string.IsNullOrWhiteSpace(question) || string.IsNullOrEmpty(question))
        {
            yield return "Question cannot be empty.";
            yield break;
        }
        
        if (question.Length > 50)
        {
            yield return "Question cannot exceed 50 characters.";
        }
    }
    
    public static IEnumerable<string> AnswerValidation(string answer)
    {
        if (string.IsNullOrWhiteSpace(answer) || string.IsNullOrEmpty(answer))
        {
            yield return "Answer cannot be empty.";
            yield break;
        }
        
        if (answer.Length > 50)
        {
            yield return "Answer cannot exceed 50 characters.";
        }
    }
}