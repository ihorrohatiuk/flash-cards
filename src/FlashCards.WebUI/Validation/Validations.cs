using System.Text.RegularExpressions;

namespace FlashCards.WebUI.Validation;

public static class Validations
{
    public static IEnumerable<string> EmailValidation(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            yield return "Email is required.";
            yield break;
        }

        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            yield return "Invalid email format.";
        if (email.Length > 50)
            yield return "Email cannot exceed 50 characters.";
    }

    public static IEnumerable<string> FirstNameValidation(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            yield return "First name is required.";
            yield break;
        }
        
        if (firstName.Length > 50)
            yield return "First name cannot exceed 50 characters.";
    }

    public static IEnumerable<string> LastNameValidation(string lastName)
    {
        if (string.IsNullOrEmpty(lastName))
        {
            yield return "Last name is required.";
            yield break;
        }
        
        if (lastName.Length > 50)
            yield return "Last name cannot exceed 50 characters.";
    }
    
    public static IEnumerable<string> PasswordValidation(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            yield return "Password is required";
            yield break;
        }
        
        if (password.Length is < 8 or > 100)
            yield return "Password must be at least 8 characters and no more than 100 characters.";
    }
}