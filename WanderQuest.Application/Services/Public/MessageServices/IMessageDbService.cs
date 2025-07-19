using Microsoft.EntityFrameworkCore;
using System;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Services.Public.MessageServices
{
    public interface IMessageDbService
    {
        Task SaveMessageAsync(Message message);

        Task<List<Message>> GetMessagesBetweenAsync(string user1Id, string user2Id);
        //Task<List<string>> GetContactedUsersAsync(string userId);
        Task<List<string>> GetContactedUsersAsync(string userId);
        Task DeleteAllMessages(string userOneName, string userTwoName);
    }
}
