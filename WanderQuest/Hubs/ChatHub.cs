using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WanderQuest.Application.Implementations.Public.MessageServices;
using WanderQuest.Application.Services.ChatGpt;
using WanderQuest.Application.Services.Public.MessageServices;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Hubs
{
    public class ChatHub : Hub
    {
        // Aktif kullanıcıları tutan thread-safe dictionary
        private static ConcurrentDictionary<string, string> ConnectedUsers = new();

        private readonly IMessageDbService _messageDbService;
        private readonly IChatGptService _chatGptService; 
        private readonly IOnlineUserService _onlineUserService;

        public ChatHub(IMessageDbService messageDbService, IChatGptService chatGptService, IOnlineUserService onlineUserService)
        {
            _messageDbService = messageDbService;
            _chatGptService = chatGptService;
            _onlineUserService = onlineUserService;
        }

        public override Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            _onlineUserService.UserConnected(userId, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.UserIdentifier;
            _onlineUserService.UserDisconnected(userId);
            return base.OnDisconnectedAsync(exception);
        }

        // Kullanıcının online olup olmadığını kontrol eden metod (istenirse dışarıya açılabilir)
        public static bool IsUserOnline(string userId)
        {
            return ConnectedUsers.ContainsKey(userId);
        }

        public async Task SendMessage(string receiverId, string messageText)
        {
            string senderId = Context.UserIdentifier;

            var message = new Message
            {
                SendUserId = senderId,
                ReceiverUserId = receiverId,
                Text = messageText,
                SentAt = DateTime.UtcNow
            };

            await _messageDbService.SaveMessageAsync(message);

            // Alıcıya mesajı gönder
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, messageText, message.SentAt);

            // Gönderene de kendi mesajını göster
            await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, messageText, message.SentAt);
        }
    }
}
