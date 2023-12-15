﻿using api.filters;
using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Services;

namespace api.Controllers;

[RequireAuthentication]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly BlobService _blobService;
    
    public UserController(UserService userService, BlobService blobService)
    {
        _userService = userService;
        _blobService = blobService;
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
    [Route("/api/users/{id}")]
    public IActionResult UpdateUser([FromForm] Users user, IFormFile? profilePicture, [FromQuery]string? password, [FromRoute] int id)
    {
        if (!HttpContext.GetSessionData().IsAdmin)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        if (profilePicture?.Length > 10 * 1024 * 1024) return StatusCode(StatusCodes.Status413PayloadTooLarge);
        var session = HttpContext.GetSessionData()!;
        string? pictureUrl = null;
        if (profilePicture != null)
        {
            pictureUrl = _userService.GetUserById(session.UserId).Picture;
            using var pictureTransform = new ImageTransform(profilePicture.OpenReadStream())
                .Resize(200, 200)
                .FixOrientation()
                .RemoveMetadata()
                .Jpeg();
            pictureUrl = _blobService.Save("userpictures", pictureTransform.ToStream(), "image/jpeg", pictureUrl);
        }
        user.UserID = id;
        user.Picture = pictureUrl;
        return Ok(_userService.UpdateUser(user, password));
    }
}