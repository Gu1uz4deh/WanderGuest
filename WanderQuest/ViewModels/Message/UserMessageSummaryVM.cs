using System;

namespace WanderQuest.ViewModels.Message
{
    public class UserMessageSummaryVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; } // username göstermek için
        public string LastMessageText { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
