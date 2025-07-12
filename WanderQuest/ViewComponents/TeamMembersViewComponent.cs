using Microsoft.AspNetCore.Mvc;
using System;
using WanderQuest.Application.Services.Public;

namespace WanderQuest.ViewComponents
{
    public class TeamMembersViewComponent : ViewComponent
    {
        private readonly ITeamMembersQueyService _teamMemberQueryService;

        public TeamMembersViewComponent(ITeamMembersQueyService teamMemberQueryService)
        {
            _teamMemberQueryService = teamMemberQueryService;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teamMembers = await _teamMemberQueryService.GetAll();
            return View(teamMembers);
        }
    }
}
