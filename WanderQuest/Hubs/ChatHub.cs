using Microsoft.AspNetCore.SignalR;
using System;
using WanderQuest.Application.Implementations.Public.MessageServices;
using WanderQuest.Application.Services.Public.MessageServices;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Hubs
{
    public class ChatHub : Hub
    {
        //public async Task SendMessage(string user, string message)
        //{
        //    // Tüm client'lara mesaj yayınla
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}
        private readonly IMessageDbService _messageDbService;
        public ChatHub(IMessageDbService messageDbService)
        {
            _messageDbService = messageDbService;
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

            // 2. Alıcıya mesajı gönder
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, messageText, message.SentAt);

            // 3. Gönderene de kendi mesajını göster (istersen bunu kaldırabilirsin)
            await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, messageText, message.SentAt);
        }

        // 4. Kullanıcı bağlantı sağladığında UserIdentifier ayarlanmalı
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
