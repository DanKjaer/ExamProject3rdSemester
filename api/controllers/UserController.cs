using api.filters;
using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

[RequireAuthentication]
[ApiController]
public class UserController
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
    public Users UpdateUser([FromForm] Users user, IFormFile? profilePicture, [FromQuery]string? password, [FromRoute] int id)
    {
        if (profilePicture?.Length > 10 * 1024 * 1024) return StatusCode(StatusCodes.Status413PayloadTooLarge);
        var session = HttpContext.GetSessionData()!;
        string? avatarUrl = null;
        if (profilePicture != null)
        {
            avatarUrl = _accountService.Get(session)?.AvatarUrl;
            using var avatarTransform = new ImageTransform(profilePicture.OpenReadStream())
                .Resize(200, 200)
                .FixOrientation()
                .RemoveMetadata()
                .Jpeg();
            avatarUrl = _blobService.Save("avatar", avatarTransform.ToStream(), "image/jpeg", avatarUrl);
        }
        user.UserID = id;
        return _userService.UpdateUser(user, password);
    }
}