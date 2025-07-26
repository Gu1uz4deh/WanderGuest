using System;

namespace WanderQuest.Application.DTO
{
    public class UserChatOverviewDto
    {
        public string Username { get; set; } // username göstermek için
        public string LastMessageText { get; set; }
        public DateTime LastMessageTime { get; set; }
        public bool IsOnline { get; set; }

    }
}
