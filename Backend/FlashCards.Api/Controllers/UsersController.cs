using System.Collections.Generic;
using FlashCards.Api.Data;
using FlashCards.Api.Models;
using FlashCards.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(AppDbContext context)
    {
        _userService = new UserService(context);
    }

    [HttpGet(Name = "GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        return _userService.GetAllUsers();
    }

    [HttpGet("{id}", Name = "GetUser")]
    public User? Get(int id)
    {
        return _userService.GetUserById(id);
    }
    
    [HttpPost(Name = "AddUser")]
    public IActionResult AddUser(User user)
    {
        _userService.AddUser(user);
        return Ok(user);
    }
}