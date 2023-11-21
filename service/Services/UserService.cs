using infrastructure.datamodels;
using infrastructure.Repositories;

namespace service.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Users GetUserById(int userId)
    {
        return _userRepository.GetUserById(userId);
    }

    public IEnumerable<Users> GetUsersForFeed()
    {
        return _userRepository.GetUsersForFeed();
    }
}