using System;

namespace WanderQuest.Application.Services.ChatGpt
{
    public interface IChatGptService
    {
        Task<string> AskChatGptAsync(string userMessage);
    }
}
