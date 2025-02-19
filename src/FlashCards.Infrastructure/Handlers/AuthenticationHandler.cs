using System.Net.Http.Headers;
using FlashCards.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace FlashCards.Infrastructure.Handlers;

public class AuthenticationHandler : DelegatingHandler
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IConfiguration _configuration;
    
    public AuthenticationHandler(IAuthenticationService authenticationService, IConfiguration configuration)
    {
        _authenticationService = authenticationService;
        _configuration = configuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var jwt = await _authenticationService.GetJwtAsync();
        var isToServer = request.RequestUri?.AbsoluteUri.StartsWith(_configuration["ServerUrl"] ?? "") ?? false;

        if (isToServer && !string.IsNullOrEmpty(jwt))
            request.Headers.Authorization = new AuthenticationHeaderValue("AccessToken", jwt);
        
        return await base.SendAsync(request, cancellationToken);
    }
}