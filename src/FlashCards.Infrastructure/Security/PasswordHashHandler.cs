using System.Security.Cryptography;
using System.Text;

namespace FlashCards.Infrastructure.Security;

public static class PasswordHashHandler
{
    public static string PasswordToHash(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        
        byte[] hashBytes = sha256.ComputeHash(inputBytes);
        
        StringBuilder hashBuilder = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            hashBuilder.Append(b.ToString("x2"));
        }

        return hashBuilder.ToString();
    }

    public static bool Verify(string password, string passwordHash)
    {
        return string.Equals(passwordHash, PasswordToHash(password));
    }
}