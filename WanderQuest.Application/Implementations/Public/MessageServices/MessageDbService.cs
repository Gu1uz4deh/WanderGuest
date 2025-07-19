using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
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
