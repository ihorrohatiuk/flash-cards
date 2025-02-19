using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Blazored.SessionStorage;
using FlashCards.Core.Application.Dtos;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace FlashCards.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public AuthenticationService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async ValueTask<string> GetJwtAsync()
    {
        throw new NotImplementedException("GetJwtAsync is not implemented");
    }

    public async Task LogoutAsync()
    {
        throw new NotImplementedException("Logging out is not implemented");
    }

    public static string GetUsername(string jwtToken)
    {
        var jwt = new JwtSecurityToken(jwtToken);
        
        return jwt.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
    }
    
    public async Task<DateTime> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/Authentication/Login")
        {
            Content = JsonContent.Create(loginRequestDto)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include); // to save cookies on client.
        
        var response = await _httpClientFactory
            .CreateClient("ServerApi")
            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        
        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Login failed.");
        
        var content = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        
        if (content == null)
            throw new InvalidDataException("Invalid data was returned as a login response.");
        
        return content.AccessTokenExpiration;
    }
}