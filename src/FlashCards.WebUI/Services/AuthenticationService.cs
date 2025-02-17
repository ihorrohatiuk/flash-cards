using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.SessionStorage;
using FlashCards.WebUI.Models;

namespace FlashCards.WebUI.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISessionStorageService _sessionStorageService;
    
    private const string JwtKey = nameof(JwtKey);
    
    private string? _jwtCache;
    
    public event Action<string?>? LoginChanged;
    
    public AuthenticationService(IHttpClientFactory factory, ISessionStorageService sessionStorageService)
    {
        _httpClientFactory = factory;
        _sessionStorageService = sessionStorageService;
    }

    public async ValueTask<string> GetJwtAsync()
    {
        if (string.IsNullOrEmpty(_jwtCache))
            _jwtCache = await _sessionStorageService.GetItemAsync<string>(JwtKey);
        
        return _jwtCache;
    }

    public async Task LogoutAsync()
    {
        await _sessionStorageService.RemoveItemAsync(JwtKey);
        _jwtCache = null;
        LoginChanged?.Invoke(null);
    }

    public static string GetUsername(string jwtToken)
    {
        var jwt = new JwtSecurityToken(jwtToken);
        
        return jwt.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
    }
    
    public async Task<DateTime> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var responce = await _httpClientFactory.CreateClient("ServerApi")
            .PostAsync("api/Authentication/Login", JsonContent.Create(loginRequestDto));

        if (!responce.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Login failed.");
        
        var content = await responce.Content.ReadFromJsonAsync<LoginResponseDto>();
        
        if (content == null)
            throw new InvalidDataException("Invalid data was returned as a login response.");
        
        await _sessionStorageService.SetItemAsync(JwtKey, content.AccessToken);
        
        LoginChanged?.Invoke(GetUsername(content.AccessToken));
        
        return content.Expiration;
    }
}