using System.Diagnostics.CodeAnalysis;
using infrastructure.datamodels;
using infrastructure.Repositories;
using service.PasswordHashing;

namespace service.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly HashingArgon2id _hashingArgon2Id;
    private DateTime? _lastCheck;
    
    public UserService(UserRepository userRepository, HashingArgon2id hashingArgon2Id)
    {
        _userRepository = userRepository;
        _hashingArgon2Id = hashingArgon2Id;
        UserDisableAndDeletion();
    }
    
    public Users GetUserById(int userId)
    {
        UserDisableAndDeletion();
        return _userRepository.GetUserById(userId);
    }

    public IEnumerable<Users> GetUsers()
    {
        UserDisableAndDeletion();
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
        UserDisableAndDeletion();
        if (password != null)
        {
            var salt = _hashingArgon2Id.GenerateSalt();
            var hash = _hashingArgon2Id.HashPassword(password, salt);
            _userRepository.UpdatePassword(hash, salt, user.UserID);
        }
        
        var newUserObject = _userRepository.UpdateUser(user);

        if (user.Disabled == true && newUserObject.Disabled == false)
        {
            _userRepository.SetDisableUser(user);
        }
        else if (user.ToBeDisabledDate != null)
        {
            _userRepository.SetToBeDisabledDate(user);
        } 
        else if (user.Disabled == false && newUserObject.Disabled == true)
        {
            _userRepository.SetDisableUser(user);
        }
        return _userRepository.GetUserById(newUserObject.UserID.Value);
    }

    public void UserDisableAndDeletion()
    {
        if (_lastCheck == null)
        {
            _userRepository.CheckUsersToBeDisabled();
            _userRepository.CheckUsersToBeDeleted();
            _lastCheck = DateTime.Now;
            
        }
        else
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan timeSinceLastCheck = currentTime - _lastCheck.Value;
            TimeSpan threshold = TimeSpan.FromDays(1);

            if (timeSinceLastCheck < threshold)
            {
                _userRepository.CheckUsersToBeDisabled();
                _userRepository.CheckUsersToBeDeleted();
            }
        }
    }
}