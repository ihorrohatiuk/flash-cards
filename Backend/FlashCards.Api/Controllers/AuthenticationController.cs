using System;
using System.Threading.Tasks;
using FlashCards.Api.Data.Dtos;
using FlashCards.Api.Data.Models;
using FlashCards.Api.Infrastructure;
using FlashCards.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers;

[ApiController]
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
    public async Task<IActionResult> AddUserAsync(UserRegistrationDto userDto) 
    {
        var result = await _userService.AddAsync(userDto);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        
        return Conflict(result.Message);
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUserAsync(UserLoginRequestDto userLoginRequestDto)
    {
        var authenticationResult = await _userService.AuthenticateAsync(userLoginRequestDto);
        if (!authenticationResult.Success)
        {
            return Unauthorized(authenticationResult.Message);
        }
        var token = authenticationResult.Data?.AccessToken;
        
        Response.Cookies.Append("AccessToken", token);
        
        return Ok();
    }
}