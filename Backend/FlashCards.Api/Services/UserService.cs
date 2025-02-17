using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Api.Data;
using FlashCards.Api.Data.Dtos;
using FlashCards.Api.Data.Models;
using FlashCards.Api.Data.Repositories;
using FlashCards.Api.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private JwtProvider _jwtProvider;
    
    public UserService(AppDbContext context, JwtProvider jwtProvider) //jwt provider shouldnt be here
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

    public async Task<Result<User>> AddAsync(UserRegistrationDto userDto)
    {
        if (_userRepository.Exists(userDto.Email).Result)
        {
            return new Result<User>(false, $"Email {userDto.Email} already exists.");
        }
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Role = RolesType.User,
            PasswordHash = PasswordHashHandler.PasswordToHash(userDto.Password),
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

    public async Task<Result<UserLoginResponseDto>> AuthenticateAsync(UserLoginRequestDto? userLoginRequestDto)
    {
        // email check
        if (userLoginRequestDto == null || !_userRepository.Exists(userLoginRequestDto.Email).Result)
        {
            return new Result<UserLoginResponseDto>(false, $"Email or password is incorrect.");
        }
        
        var user = await _userRepository.GetByEmailAsync(userLoginRequestDto.Email);
        // password check
        if (!PasswordHashHandler.Verify(userLoginRequestDto.Password, user.PasswordHash))
        {
            return new Result<UserLoginResponseDto>(false, $"Email or password is incorrect.");
        } 
        
        // return token
        var token = _jwtProvider.GenerateJwtToken(user);
        var userLoginResponseDto = new UserLoginResponseDto
        {
            AccessToken = token,
            // Add exipation time
        };
        
        return new Result<UserLoginResponseDto>(userLoginResponseDto, true, $"User {user.Email} successfully logged in.");
    }
}