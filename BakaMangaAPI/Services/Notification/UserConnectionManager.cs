
namespace BakaMangaAPI.Services.Notification;

public class UserConnectionManager : IUserConnectionManager
{
    private static readonly Dictionary<string, List<string>> _userConnectionMap = new();
    private static readonly string _userConnectionMapLocker = string.Empty;

    public List<string> GetUserConnections(string userId)
    {
        var conn = new List<string>();
        lock (_userConnectionMapLocker)
        {
            conn = _userConnectionMap[userId];
        }
        return conn;
    }

    public List<string> GetManyUsersConnections(string[] userIds)
    {
        var conn = new List<string>();
        lock (_userConnectionMapLocker)
        {
            foreach (var userId in userIds)
            {
                var result = _userConnectionMap.TryGetValue(userId, out var singleUserConn);
                if (result)
                {
                    conn.AddRange(singleUserConn!);
                }
            }
        }
        return conn;
    }

    public void KeepUserConnection(string userId, string connectionId)
    {
        lock (_userConnectionMapLocker)
        {
            if (!_userConnectionMap.ContainsKey(userId))
            {
                _userConnectionMap[userId] = new List<string>();
            }
            if (!_userConnectionMap[userId].Contains(connectionId))
            {
                _userConnectionMap[userId].Add(connectionId);
            }
        }
    }

    public void RemoveUserConnection(string connectionId)
    {
        lock (_userConnectionMapLocker)
        {
            foreach (var userId in _userConnectionMap.Keys)
            {
                if (_userConnectionMap[userId].Contains(connectionId))
                {
                    _userConnectionMap[userId].Remove(connectionId);
                    break;
                }
            }
        }
    }
}
