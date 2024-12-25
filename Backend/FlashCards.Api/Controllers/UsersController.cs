using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Api.Data;
using FlashCards.Api.Data.Dtos;
using FlashCards.Api.Data.Models;
using FlashCards.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    /*public UsersController(AppDbContext context)
    {
        _userService = new UserService(context);
    }

    [HttpGet(Name = "GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        return _userService.GetAll();
    }

    [HttpGet("{id}", Name = "GetUser")]
    public async Task<User?> Get(Guid id)
    {
        return await _userService.GetByIdAsync(id);
    }*/
}