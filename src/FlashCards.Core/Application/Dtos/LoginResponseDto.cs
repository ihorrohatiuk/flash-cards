namespace FlashCards.Core.Application.Dtos;

public class LoginResponseDto
{
    public string? AccessToken { get; init; }
    public DateTime AccessTokenExpiration { get; init; }
}