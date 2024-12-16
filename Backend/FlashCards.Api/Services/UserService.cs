using System.Collections.Generic;
using FlashCards.Api.Data;
using FlashCards.Api.Data.Repositories;
using FlashCards.Api.Models;

namespace FlashCards.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(AppDbContext context)
    {
        _userRepository = new UserRepository(context);    
    }
    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
    }

    public User? GetUserById(int id)
    {
        return _userRepository.GetUserById(id);
    }

    public void AddUser(User user)
    {
        // TODO: Add validation
        _userRepository.AddUser(user);
    }

    public void UpdateUser(User user)
    {
        // TODO: Add validation
        _userRepository.UpdateUser(user);
    }

    public void DeleteUser(int id)
    {
        _userRepository.DeleteUser(id);
    }
}