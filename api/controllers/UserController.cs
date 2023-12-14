using infrastructure.datamodels;
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
        Console.WriteLine("HALLLØJ");
        return _userService.CreateUser(user, password);
    }

    [HttpPut]
    [Route("/api/users/{id}")]
    public Users UpdateUser([FromBody] Users user, [FromQuery]string? password, [FromRoute] int id)
    {
        user.UserID = id;
        Console.WriteLine("FEJLEN STÅR HER VENNER ~~~~~~~~~~~~" + user.UserID);
        return _userService.UpdateUser(user, password);
    }
}