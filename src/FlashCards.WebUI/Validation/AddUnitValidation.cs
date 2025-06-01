using FlashCards.Core.Domain.Constants;

namespace FlashCards.WebUI.Validation;

public static class UnitValidation
{
    public static IEnumerable<string> UnitNameValidation(string unitName)
    {
        if (string.IsNullOrWhiteSpace(unitName) || string.IsNullOrEmpty(unitName))
        {
            yield return "Unit name cannot be empty.";
            yield break;
        }
        
        if (unitName.Length is < 2 or > AppConstants.MaxUnitNameLength)
        {
            yield return $"Unit name must be between 2 and {AppConstants.MaxUnitNameLength} characters long.";
        }
    }
    
    public static IEnumerable<string> UnitSubjectValidation(string unitSubject)
    {
        if (string.IsNullOrWhiteSpace(unitSubject) || string.IsNullOrEmpty(unitSubject))
        {
            yield return "Unit subject cannot be empty.";
            yield break;
        }
        
        if (unitSubject.Length is < 1 or > AppConstants.MaxUnitNameLength)
        {
            yield return $"Unit subject must be between 1 and {AppConstants.MaxUnitNameLength} characters long.";
        }
    }

    public static IEnumerable<string> QuestionValidation(string question)
    {
        if (string.IsNullOrWhiteSpace(question) || string.IsNullOrEmpty(question))
        {
            yield return "Question cannot be empty.";
            yield break;
        }
        
        if (question.Length > AppConstants.MaxQuestionLength)
        {
            yield return $"Question cannot exceed {AppConstants.MaxQuestionLength} characters.";
        }
    }
    
    public static IEnumerable<string> AnswerValidation(string answer)
    {
        if (string.IsNullOrWhiteSpace(answer) || string.IsNullOrEmpty(answer))
        {
            yield return "Answer cannot be empty.";
            yield break;
        }
        
        if (answer.Length > AppConstants.MaxAnswerLength)
        {
            yield return $"Answer cannot exceed {AppConstants.MaxAnswerLength} characters.";
        }
    }
}