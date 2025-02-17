namespace FlashCards.WebUI.Models;

public class LoginResponseDto
{
    public required string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
}