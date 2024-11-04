using System;
using System.Collections.Generic;
using FlashCards.Api.Data;
using FlashCards.Api.Data.Repositories;
using FlashCards.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlashCards.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _userRepository;

    public UsersController(AppDbContext context, ILogger<UsersController> logger)
    {
        _logger = logger;
        _userRepository = new UserRepository(context);
    }

    [HttpGet(Name = "GetUsers")]
    public IEnumerable<User> Get()
    {
        return _userRepository.GetAllUsers();
    }

    [HttpGet("{id}", Name = "GetUser")]
    public User Get(int id)
    {
        return _userRepository.GetUserById(id);
    }
    
    [HttpPost(Name = "AddUser")]
    public IActionResult Post(User user)
    {
        try
        {
            _userRepository.AddUser(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
        return Ok(user);
    }
}