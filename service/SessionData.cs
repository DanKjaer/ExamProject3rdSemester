using infrastructure.datamodels;

namespace service;

public class SessionData
{
    public required int UserId { get; init; }
    public required bool IsAdmin { get; init; }

    public static SessionData FromUser(Users user)
    {
        return new SessionData { UserId = user.UserID.Value, IsAdmin = (user.UserType == UserType.Manager) };
    }

    public static SessionData FromDictionary(Dictionary<string, object> dict)
    {
        return new SessionData { UserId = (int)dict[Keys.UserId], IsAdmin = (bool)dict[Keys.IsAdmin] };
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object> { { Keys.UserId, UserId }, { Keys.IsAdmin, IsAdmin } };
    }

    public static class Keys
    {
        public const string UserId = "u";
        public const string IsAdmin = "a";
    }
}