using api.transferModels;
using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Services;

namespace api.Controllers;
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationController(JwtService jwtService, AuthenticationService authenticationService)
    {
        _jwtService = jwtService;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("/api/Login")]
    public ResponseToken Login([FromBody] UserLogin userLogin)
    {
        var user = _authenticationService.Authenticate(userLogin);
        if (user == null)
        {
            return new ResponseToken()
            {
                token = "Failure to Authenticate"
            };
        }

        var token = _jwtService.IssueToken(SessionData.FromUser(user!));
        var result = new ResponseToken()
        {
            token = token
        };
        return result;
    }
}