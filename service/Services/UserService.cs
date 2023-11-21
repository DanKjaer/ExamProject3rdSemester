using System.Diagnostics.CodeAnalysis;
using infrastructure.datamodels;
using infrastructure.Repositories;
using service.PasswordHashing;

namespace service.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly HashingArgon2id _hashingArgon2Id;
    
    public UserService(UserRepository userRepository, HashingArgon2id hashingArgon2Id)
    {
        _userRepository = userRepository;
        _hashingArgon2Id = hashingArgon2Id;
    }
    
    public Users GetUserById(int userId)
    {
        return _userRepository.GetUserById(userId);
    }

    public IEnumerable<Users> GetUsers()
    {
        return _userRepository.GetUsers();
    }

    public Users CreateUser(Users user, string password)
    {
        var salt = _hashingArgon2Id.GenerateSalt();
        var hash = _hashingArgon2Id.HashPassword(password, salt);

        var newUser = _userRepository.CreateUser(user);
        _userRepository.CreatePassword(hash, salt, newUser.UserID);
        return newUser;
    }

    public Users UpdateUser(Users user, string? password)
    {
        if (password != null)
        {
            var salt = _hashingArgon2Id.GenerateSalt();
            var hash = _hashingArgon2Id.HashPassword(password, salt);
            _userRepository.UpdatePassword(hash, salt, user.UserID);
        }

        return _userRepository.UpdateUser(user);
    }
}