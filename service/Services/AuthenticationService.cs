using infrastructure.datamodels;
using infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using service.PasswordHashing;

namespace service.Services;

public class AuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;
    private readonly AuthenticateRepository _authenticateRepository;
    private readonly UserRepository _userRepository;

    public AuthenticationService(ILogger<AuthenticationService> logger, AuthenticateRepository authenticateRepository, 
                                    UserRepository userRepository)
    {
        _logger = logger;
        _authenticateRepository = authenticateRepository;
        _userRepository = userRepository;
    }

    public Users? Authenticate(UserLogin userLogin)
    {
        try
        {
            var pwHash = _authenticateRepository.GetUserByEmail(userLogin.Email);
            var hashAlgorithm = new HashingArgon2id();
            var isValid =
                hashAlgorithm.VerifyHashedPassword(userLogin.Password, pwHash.PasswordHashed, pwHash.PasswordSalt);
            if (isValid)
            {
                return _userRepository.GetUserById(pwHash.UserID);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Authentication error: {Message}", e);
        }

        return null;
    }
    
    public Users? Get(SessionData data)
    {
        return _userRepository.GetUserById(data.UserId);
    }
}
