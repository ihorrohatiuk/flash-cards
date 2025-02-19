using FlashCards.Core.Application.Dtos;

namespace FlashCards.Infrastructure.Services;

public interface IAuthenticationService
{
    ValueTask<string> GetJwtAsync();
    Task LogoutAsync();
    Task<DateTime> LoginAsync(LoginRequestDto loginRequestDto);
}