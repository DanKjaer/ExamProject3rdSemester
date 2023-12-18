using infrastructure.datamodels;

namespace service;

public class SessionData
{
    public required int UserId { get; init; }
    public required bool IsManager { get; init; }
    public required bool IsDyrepasser { get; init; }

    public static SessionData FromUser(Users user)
    {
        bool isManager = user.UserType == UserType.Manager;
        bool isDyrepasser = user.UserType == UserType.Dyrepasser;
        return new SessionData { UserId = user.UserID!.Value, IsManager = isManager, IsDyrepasser = isDyrepasser };
    }

    public static SessionData FromDictionary(Dictionary<string, object> dict)
    {
        return new SessionData { UserId = (int)dict[Keys.UserId], IsManager = (bool)dict[Keys.IsManager], IsDyrepasser = (bool)dict[Keys.IsDyrepasser]};
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object> { { Keys.UserId, UserId }, { Keys.IsManager, IsManager }, {Keys.IsDyrepasser, IsDyrepasser} };
    }

    public static class Keys
    {
        public const string UserId = "u";
        public const string IsManager = "a";
        public const string IsDyrepasser = "d";
    }
}