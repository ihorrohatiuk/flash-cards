using FlashCards.Core.Application.Dtos;
using FlashCards.Core.Domain.Constants;
using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.Contexts;
using FlashCards.Infrastructure.Persistence.Repositories;
using FlashCards.Infrastructure.Security;

namespace FlashCards.Infrastructure.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private JwtProvider _jwtProvider;
    
    public UserService(AppDbContext context, JwtProvider jwtProvider) //TODO jwt provider shouldnt be here, write authService for this
    {
        _userRepository = new UserRepository(context);    
        _jwtProvider = jwtProvider;
    }
    public IEnumerable<User> GetAll()
    {
        return _userRepository.GetAll();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<Result<User>> AddAsync(RegistrationRequestDto registrationRequestDto)
    {
        if (_userRepository.Exists(registrationRequestDto.Email).Result)
        {
            return new Result<User>(false, $"Email {registrationRequestDto.Email} already exists.");
        }
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = registrationRequestDto.FirstName,
            LastName = registrationRequestDto.LastName,
            Email = registrationRequestDto.Email,
            Role = RolesType.User,
            PasswordHash = PasswordHashHandler.PasswordToHash(registrationRequestDto.Password),
        };
        
        await _userRepository.AddAsync(user);
        
        return new Result<User>(true, $"User {user.Email} successfully created.");
    }

    public async Task UpdateAsync(User user) //TODO: Implement 
    {
        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public async Task<Result<LoginResponseDto>> AuthenticateAsync(LoginRequestDto? userLoginRequestDto) //TODO: Move to Authentication service
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