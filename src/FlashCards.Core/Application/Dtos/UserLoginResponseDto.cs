namespace FlashCards.Core.Application.Dtos;

public class UserLoginResponseDto
{
    public string? AccessToken { get; init; }
    public DateTime AccessTokenExpiry { get; init; }
}