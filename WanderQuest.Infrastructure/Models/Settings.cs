using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.Entity;

namespace WanderQuest.Infrastructure.Models
{
    public class Settings : IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; } 
    }
}
