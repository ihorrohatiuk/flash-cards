using FlashCards.Core.Application.Dtos;
using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.Repositories;
using FlashCards.Infrastructure.Security;

namespace FlashCards.Infrastructure.Services;

public class AuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtProvider _jwtProvider;

    public AuthenticationService(IUserRepository userRepository, JwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Result<LoginResponseDto>> AuthenticateAsync(LoginRequestDto? userLoginRequestDto)
    {
        // email check
        if (userLoginRequestDto == null || !_userRepository.Exists(userLoginRequestDto.Email).Result)
        {
            return new Result<LoginResponseDto>(false, $"Email or password is incorrect.");
        }
        
        var user = await _userRepository.GetByEmailAsync(userLoginRequestDto.Email);
        // password check
        if (!PasswordHashHandler.Verify(userLoginRequestDto.Password, user.PasswordHash))
        {
            return new Result<LoginResponseDto>(false, $"Email or password is incorrect.");
        } 
        
        // return token
        var token = _jwtProvider.GenerateJwtToken(user);
        var userLoginResponseDto = new LoginResponseDto
        {
            AccessToken = token,
        };
        
        return new Result<LoginResponseDto>(userLoginResponseDto, true, $"User {user.Email} successfully logged in.");
    }
}