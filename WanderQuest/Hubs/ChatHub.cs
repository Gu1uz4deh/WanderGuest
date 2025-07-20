using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using WanderQuest.Application.Implementations.Public.MessageServices;
using WanderQuest.Application.Services.ChatGpt;
using WanderQuest.Application.Services.Public.MessageServices;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageDbService _messageDbService;
        private readonly IChatGptService _chatGptService;
        public ChatHub(IMessageDbService messageDbService, IChatGptService chatGptService)
        {
            _messageDbService = messageDbService;
            _chatGptService = chatGptService;
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

        //public async Task SendMessage(string receiverId, string messageText)
        //{
        //    string senderId = Context.UserIdentifier;

        //    var message = new Message
        //    {
        //        SendUserId = senderId,
        //        ReceiverUserId = receiverId,
        //        Text = messageText,
        //        SentAt = DateTime.UtcNow
        //    };

        //    //await _context.Messages.AddAsync(message);
        //    //await _context.SaveChangesAsync();
        //    await _messageDbService.SaveMessageAsync(message);

        //    // 2. Mesajı qarşı tərəfə yolla
        //    await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, messageText, message.SentAt);
        //    //await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, messageText, message.SentAt);

        //    // 3. Əgər qarşı tərəf ChatGpt isə
        //    if (receiverId == "ChatGpt")
        //    {
        //        var reply = await _chatGptService.AskChatGptAsync(messageText);

        //        // 4. GPT cavabını DB-ə yaz
        //        var gptReplyMessage = new Message
        //        {
        //            SendUserId = "ChatGpt",
        //            ReceiverUserId = senderId,
        //            Text = reply,
        //            SentAt = DateTime.UtcNow
        //        };

        //        //await _context.Messages.AddAsync(gptReplyMessage);
        //        //await _context.SaveChangesAsync();
        //        await _messageDbService.SaveMessageAsync(gptReplyMessage);

        //        // 5. Cavabı istifadəçiyə yolla
        //        await Clients.User(senderId).SendAsync("ReceiveMessage", "ChatGpt", reply, gptReplyMessage.SentAt);
        //    }

        //}
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        
    }

}
