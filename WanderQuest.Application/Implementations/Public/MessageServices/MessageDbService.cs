using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using WanderQuest.Application.DTO;
using WanderQuest.Application.Services.Public.MessageServices;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Implementations.Public.MessageServices
{
    public class MessageDbService : IMessageDbService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public MessageDbService(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task SaveMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetMessagesBetweenAsync(string userOneName, string userTwoName)
        {
            return await _context.Messages
                .Where(m =>
                    (m.SendUserId == userOneName && m.ReceiverUserId == userTwoName) ||
                    (m.SendUserId == userTwoName && m.ReceiverUserId == userOneName))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
        public async Task<List<string>> GetContactedUsersAsync(string username)
        {
            var contactedUsernames = await _context.Messages
                .Where(m => m.SendUserId == username || m.ReceiverUserId == username)
                .Select(m => m.SendUserId == username ? m.ReceiverUserId : m.SendUserId)
                .Distinct()
                .ToListAsync();

            //return await _context.Users
            //    .Where(n => contactedUsers.Contains(n.UserName))
            //    .ToListAsync();
            //var users = await _userManager.Users.Where(n => )
            return contactedUsernames;
        }

        public async Task<List<UserChatOverviewDto>> GetLastMessageSummary(string username)
        {
            List<UserChatOverviewDto> userChatLists = new List<UserChatOverviewDto>();

            // 1. Kullanıcının konuştuğu diğer kişileri bul
            //var otherUsers = await _context.Messages
            //    .Where(m => m.SendUserId == userOneName || m.ReceiverUserId == userOneName)
            //    .Select(m => m.SendUserId == userOneName ? m.ReceiverUserId : m.SendUserId)
            //    .Distinct()
            //    .ToListAsync();
            var otherUsers = await GetContactedUsersAsync(username);

            // 2. Her kişi için en son mesajı al ve listeye ekle
            foreach (var otherUser in otherUsers)
            {
                var lastMessage = await _context.Messages
                    .Where(m =>
                        (m.SendUserId == username && m.ReceiverUserId == otherUser) ||
                        (m.SendUserId == otherUser && m.ReceiverUserId == username))
                    .OrderByDescending(m => m.SentAt)
                    .FirstOrDefaultAsync();

                if (lastMessage != null)
                {
                    userChatLists.Add(new UserChatOverviewDto
                    {
                        Username = otherUser,
                        LastMessageText = lastMessage.Text,
                        LastMessageTime = lastMessage.SentAt
                    });
                }
            }
            // 3. En son mesaj tarihine göre sıralayıp döndür
            return userChatLists.OrderByDescending(x => x.LastMessageTime).ToList();
        }
        public async Task DeleteAllMessages(string userOneName, string userTwoName)
        {
            var allMessages = await _context.Messages
                .Where(m =>
                    (m.SendUserId == userOneName && m.ReceiverUserId == userTwoName) ||
                    (m.SendUserId == userTwoName && m.ReceiverUserId == userOneName))
                .ToListAsync();

            _context.Messages.RemoveRange(allMessages);
            await _context.SaveChangesAsync();
        }
    }
}
