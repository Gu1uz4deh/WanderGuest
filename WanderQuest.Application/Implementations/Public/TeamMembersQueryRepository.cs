using Microsoft.EntityFrameworkCore;
using System;
using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Implementations.Public
{
    public class TeamMembersQueryRepository : ITeamMembersQueyService
    {
        private readonly AppDbContext _context;
        public TeamMembersQueryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TeamMember>> GetAll()
        {
            var teamMembers = await _context.TeamMembers.Where(n => !n.IsDeleted)
                                              .Include(n => n.TeamMemberImages)
                                              .ThenInclude(n => n.Image)
                                              .ToListAsync();

            return teamMembers;
        }

        public async Task<TeamMember> GetById(int id)
        {
            var teamMember = await _context.TeamMembers.Where(n => !n.IsDeleted && n.Id == id)
                                        .Include(n => n.TeamMemberImages)
                                        .ThenInclude(n => n.Image)
                                        .FirstOrDefaultAsync();
            return teamMember;
        }
        public async Task<List<TeamMember>> GetPaged(int skip = 0, int take = 10)
        {
            var teamMembers = await _context.TeamMembers.Where(n => !n.IsDeleted)
                                              .Include(n => n.TeamMemberImages)
                                              .ThenInclude(n => n.Image)
                                              .Skip(skip)
                                              .Take(take)
                                              .ToListAsync();
            return teamMembers;
        }
    }
}
