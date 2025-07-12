using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WanderQuest.Infrastructure.Models
{
    public class TeamMemberImages
    {
        public int Id { get; set; }
        public int TeamMemberId { get; set; }
        public TeamMember TeamMember { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
