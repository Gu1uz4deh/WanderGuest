using System;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Services.Public
{
    public interface ITeamMembersQueyService
    {
        Task<List<TeamMember>> GetAll();
        Task<TeamMember> GetById(int id);
        Task<List<TeamMember>> GetPaged(int skip = 0, int take = 4);
    }
}
