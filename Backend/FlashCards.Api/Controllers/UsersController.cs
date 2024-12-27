using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Api.Data;
using FlashCards.Api.Data.Dtos;
using FlashCards.Api.Data.Models;
using FlashCards.Api.Infrastructure;
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

    public UsersController(AppDbContext context, JwtProvider jwtProvider)
    {
        _userService = new UserService(context, jwtProvider);
    }

    [HttpGet(Name = "GetUsers")]
    [Authorize(Roles = RolesType.Admin)]
    public IEnumerable<User> GetUsers()
    {
        return _userService.GetAll();
    }
    
    [HttpGet("{id}", Name = "GetUser")]
    [Authorize(Roles = RolesType.User)]
    public async Task<User?> Get(Guid id)
    {
        return await _userService.GetByIdAsync(id);
    }
}