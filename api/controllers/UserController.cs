﻿using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

[ApiController]
public class UserController
{
    private readonly UserService _userService;
    
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("/api/users")]
    public IEnumerable<Users> GetUsers()
    {
        return _userService.GetUsers();
    }

    [HttpGet]
    [Route("api/users/{id}")]
    public Users GetUser(int id)
    {
        return _userService.GetUserById(id);
    }

    [HttpPost]
    [Route("/api/users")]
    public Users CreateUser([FromBody] Users user, [FromQuery]string password)
    {
        return _userService.CreateUser(user, password);
    }

    [HttpPut]
    [Route("/api/users")]
    public Users UpdateUser([FromBody] Users user, [FromQuery]string? password)
    {
        return _userService.UpdateUser(user, password);
    }
}