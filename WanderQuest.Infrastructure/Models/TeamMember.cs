using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WanderQuest.Infrastructure.Models
{
    public class TeamMember : BaseEntity
    {
        public string FullName { get; set; }
        public string Role { get; set; }
        public List<TeamMemberImages> TeamMemberImages { get; set; }
    }
}
