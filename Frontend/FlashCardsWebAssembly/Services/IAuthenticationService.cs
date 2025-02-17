using FlashCardsWebAssembly.Models;

namespace FlashCardsWebAssembly.Services;

public interface IAuthenticationService
{
    event Action<string?>? LoginChanged;
    ValueTask<string> GetJwtAsync();
    Task LogoutAsync();
    Task<DateTime> LoginAsync(LoginRequestDto loginRequestDto);
}