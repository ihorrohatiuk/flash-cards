using System;

namespace FlashCards.Api.Data.Dtos;

public class UserLoginResponseDto
{
    public string? AccessToken { get; init; }
    public DateTime AccessTokenExpiry { get; init; }
}