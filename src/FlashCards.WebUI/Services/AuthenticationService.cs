using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using FlashCards.Core.Application.Dtos;
using FlashCards.Infrastructure.Security;
using FlashCards.Infrastructure.Services;

namespace FlashCards.WebUI.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorageService;
    
    private string? _jwtCache;
    
    private const string AccessToken = nameof(AccessToken);

    public AuthenticationService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
    {
        _httpClientFactory = httpClientFactory;
        _localStorageService = localStorageService;
    }

    public async ValueTask<string> GetJwtAsync()
    {
        if (string.IsNullOrEmpty(_jwtCache))
            _jwtCache = await _localStorageService.GetItemAsync<string>(AccessToken);

        return _jwtCache;
    }

    public async Task LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync(AccessToken);
        _jwtCache = null;
    }

    public static string GetUsername(string jwtToken)
    {
        var jwt = new JwtSecurityToken(jwtToken);
        
        return jwt.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
    }
    
    public async Task<string> GetUserId()
    {
        var jwt = new JwtSecurityToken(await GetJwtAsync());
        
        return jwt.Claims.First(claim => claim.Type == JwtClaims.UserId).Value;
    }
    
    public async Task<bool> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .PostAsync("api/Authentication/Login", JsonContent.Create(loginRequestDto));
        
        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Login failed.");
        
        var content = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        
        if (content == null)
            throw new InvalidDataException("Invalid data was returned as a login response.");
        
        await _localStorageService.SetItemAsync(AccessToken, content.AccessToken);
        
        return true;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await GetJwtAsync();
        return !string.IsNullOrEmpty(token);
    }
    
    public async Task<bool> RegisterAsync(RegistrationRequestDto registrationRequestDto)
    {
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .PostAsync("api/Authentication/Register", JsonContent.Create(registrationRequestDto));
        
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Failed to register user. " + response.Content.ReadAsStringAsync().Result);
        
        return true;
    }
    
    public bool IsTokenExpired(string token)
    {
        var jwt = new JwtSecurityToken(token);
        var exp = jwt.ValidTo; // UTC

        return exp < DateTime.UtcNow;
    }
}