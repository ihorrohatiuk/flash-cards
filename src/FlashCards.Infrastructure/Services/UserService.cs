using FlashCards.Core.Application.Dtos;
using FlashCards.Core.Domain.Constants;
using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.Contexts;
using FlashCards.Infrastructure.Persistence.DataModels;
using FlashCards.Infrastructure.Persistence.Repositories;
using FlashCards.Infrastructure.Security;

namespace FlashCards.Infrastructure.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    
    public UserService(AppDbContext context) 
    {
        _userRepository = new UserRepository(context);    
    }
    public IEnumerable<UserEntity> GetAll()
    {
        return _userRepository.GetAll();
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<Result<UserEntity>> AddAsync(RegistrationRequestDto registrationRequestDto)
    {
        if (_userRepository.Exists(registrationRequestDto.Email).Result)
        {
            return new Result<UserEntity>(false, $"Email {registrationRequestDto.Email} already exists.");
        }
        
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            FirstName = registrationRequestDto.FirstName,
            LastName = registrationRequestDto.LastName,
            Email = registrationRequestDto.Email,
            Role = RolesType.User,
            PasswordHash = PasswordHashHandler.PasswordToHash(registrationRequestDto.Password),
        };
        
        await _userRepository.AddAsync(user);
        
        return new Result<UserEntity>(true, $"User {user.Email} successfully created.");
    }

    public async Task UpdateAsync(UserEntity userEntity) //TODO: Implement 
    {
        await _userRepository.UpdateAsync(userEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }
}