using System;

namespace WanderQuest.Application.Services.Public.MessageServices
{
    public interface IOnlineUserService
    {
        bool IsUserOnline(string userId);
        void UserConnected(string userId, string connectionId);
        void UserDisconnected(string userId);
    }
}
