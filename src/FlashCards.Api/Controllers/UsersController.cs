using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Core.Domain.Constants;
using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.Contexts;
using FlashCards.Infrastructure.Security;
using FlashCards.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(AppDbContext context)
    {
        _userService = new UserService(context);
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