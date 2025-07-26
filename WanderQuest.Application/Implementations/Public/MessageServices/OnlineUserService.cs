using System;
using System.Collections.Concurrent;
using WanderQuest.Application.Services.Public.MessageServices;

namespace WanderQuest.Application.Implementations.Public.MessageServices
{
    public class OnlineUserService : IOnlineUserService
    {
        private readonly ConcurrentDictionary<string, string> _connectedUsers = new();

        public bool IsUserOnline(string userId)
        {
            return _connectedUsers.ContainsKey(userId);
        }

        public void UserConnected(string userId, string connectionId)
        {
            _connectedUsers[userId] = connectionId;
        }

        public void UserDisconnected(string userId)
        {
            _connectedUsers.TryRemove(userId, out _);
        }
    }
}
