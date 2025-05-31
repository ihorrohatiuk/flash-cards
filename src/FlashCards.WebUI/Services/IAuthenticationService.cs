using FlashCards.Core.Application.Dtos;

namespace FlashCards.WebUI.Services;

public interface IAuthenticationService
{
    ValueTask<string> GetJwtAsync();
    Task LogoutAsync();
    Task<bool> LoginAsync(LoginRequestDto loginRequestDto);
    Task<bool> IsLoggedInAsync();
    Task<bool> RegisterAsync(RegistrationRequestDto registrationRequestDto);
    Task<string> GetUserId();
    bool IsTokenExpired(string token);
}