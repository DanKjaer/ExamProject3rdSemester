using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
public class UserController
{

    [HttpGet]
    [Route("/api/users")]
    public IEnumerable<Users> GetUsers()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("api/users/{id}")]
    public Users GetUser(int id)
    {
        throw new NotImplementedException();
    }
    
    
}