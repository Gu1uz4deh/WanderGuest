using System;
using WanderQuest.Core.Entity;

namespace WanderQuest.Infrastructure.Models
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
