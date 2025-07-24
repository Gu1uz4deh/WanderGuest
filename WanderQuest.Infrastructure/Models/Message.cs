using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.Entity;

namespace WanderQuest.Infrastructure.Models
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public string SendUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
