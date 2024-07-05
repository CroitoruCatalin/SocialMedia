using System.Collections.Concurrent;

public class UserConnectionManager
{
    private readonly ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();

    public void AddConnection(string userId, string connectionId)
    {
        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(connectionId))
        {
            _userConnections[userId] = connectionId;
        }

    }

    public void RemoveConnection(string userId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            _userConnections.TryRemove(userId, out _);
        }
    }

    public string GetConnectionId(string userId)
    {
        _userConnections.TryGetValue(userId, out var connectionId);
        return connectionId;
    }
}
