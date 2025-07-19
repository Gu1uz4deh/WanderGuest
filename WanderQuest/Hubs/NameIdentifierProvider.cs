using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;

namespace WanderQuest.Hubs
{
    public class NameIdentifierProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            //return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return connection.User?.Identity?.Name;
        }
    }
}
