using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.SessionStorage;
using FlashCards.Core.Application.Dtos;
using FlashCards.Infrastructure.Services;

namespace FlashCards.WebUI.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISessionStorageService _sessionStorageService;
    
    private string? _jwtCache;
    
    private const string AccessToken = nameof(AccessToken);

    public AuthenticationService(IHttpClientFactory httpClientFactory, ISessionStorageService sessionStorageService)
    {
        _httpClientFactory = httpClientFactory;
        _sessionStorageService = sessionStorageService;
    }

    public async ValueTask<string> GetJwtAsync()
    {
        if (string.IsNullOrEmpty(_jwtCache))
            _jwtCache = await _sessionStorageService.GetItemAsync<string>(AccessToken);

        return _jwtCache;
    }

    public async Task LogoutAsync()
    {
        await _sessionStorageService.RemoveItemAsync(AccessToken);
        _jwtCache = null;
    }

    public static string GetUsername(string jwtToken)
    {
        var jwt = new JwtSecurityToken(jwtToken);
        
        return jwt.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
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
        
        await _sessionStorageService.SetItemAsync(AccessToken, content.AccessToken);
        
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
}