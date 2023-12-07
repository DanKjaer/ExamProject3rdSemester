using service.Services;

namespace api.Controllers;

public class AuthenticationController
{
    private readonly JwtService _jwtService;
    private readonly AuthenticationService _authenticationService;
}