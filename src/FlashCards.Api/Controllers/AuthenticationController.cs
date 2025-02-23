using System;
using System.Threading.Tasks;
using FlashCards.Core.Application.Dtos;
using FlashCards.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserService _userService;
    
    public AuthenticationController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddUserAsync(RegistrationRequestDto registrationRequestDto) 
    {
        var result = await _userService.AddAsync(registrationRequestDto);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        
        return Conflict(result.Message);
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUserAsync(LoginRequestDto loginRequestDto)
    {
        var authenticationResult = await _userService.AuthenticateAsync(loginRequestDto);
        if (!authenticationResult.Success)
        {
            return Unauthorized(authenticationResult.Message);
        }
        var token = authenticationResult.Data?.AccessToken;
        ArgumentNullException.ThrowIfNull(token);
        
        return Ok(new LoginResponseDto
        {
            AccessToken = token
        });
    }
}