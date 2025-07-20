using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WanderQuest.Application.DTO;
using WanderQuest.Application.Services.ChatGpt;
using WanderQuest.Application.Services.Public.MessageServices;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IMessageDbService _messageService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IChatGptService _chatGptService;


        public ChatController(IMessageDbService messageService, UserManager<AppUser> userManager, IChatGptService chatGptService)
        {
            _messageService = messageService;
            _userManager = userManager;
            _chatGptService = chatGptService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(actionName: "Login", controllerName: "Account");
            }
            var myUsername = User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(myUsername);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            // Artık contacted users da username ile gelecek
            //var contacts = await _messageService.GetContactedUsersAsync(user.UserName);
            var userChatOverview = await _messageService.GetLastMessageSummary(user.UserName);


            ViewBag.MyUsername = myUsername;
            return View(userChatOverview); // View: Views/Chat/Index.cshtml
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string receiverId)
        {
            var myUsername = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(myUsername) || string.IsNullOrWhiteSpace(receiverId))
                return BadRequest();

            var messages = await _messageService.GetMessagesBetweenAsync(myUsername, receiverId);
            ViewBag.MyUsername = myUsername;

            return PartialView("_MessageHistoryPartial", messages); // View: _MessageHistoryPartial.cshtml
        }

        [HttpGet]
        public async Task<IActionResult> SearchUser(string query)
        {
            var myUsername = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(query) || string.IsNullOrWhiteSpace(myUsername))
                return Json(new List<object>());

            var results = await _userManager.Users
                .Where(u => u.UserName.Contains(query) && u.UserName != myUsername)
                .Select(u => new { u.UserName })
                //.Take(10)
                .ToListAsync();

            return Json(results);
        }

        [HttpDelete("deleteAllMessages/{userId}")]
        public async Task<IActionResult> DeleteAllMessages(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId is required");

            // silme işlemivar
            var myUsername = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(myUsername) || string.IsNullOrWhiteSpace(userId))
                return BadRequest();

            await _messageService.DeleteAllMessages(myUsername, userId);

            return RedirectToAction("Index", "Chat");

            return Ok();
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] string question)
        {
            var reply = await _chatGptService.AskChatGptAsync(question);
            return Ok(reply);
        }
    }
}









//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;
//using WanderQuest.Application.Implementations.Public.MessageServices;
//using WanderQuest.Application.Services.Public.MessageServices;
//using WanderQuest.Infrastructure.Models;

//namespace WanderQuest.Controllers
//{
//    [Authorize]
//    public class ChatController : Controller
//    {
//        private readonly IMessageDbService _messageService;
//        private readonly UserManager<AppUser> _userManager;

//        public ChatController(IMessageDbService messageService, UserManager<AppUser> userManager)
//        {
//            _messageService = messageService;
//            _userManager = userManager;
//        }
//        [AllowAnonymous]
//        public async Task<IActionResult> Index()
//        {
//            if (!User.Identity.IsAuthenticated)
//            {
//                return RedirectToAction(actionName: "Login", controllerName: "Account");
//            }
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var contacts = await _messageService.GetContactedUsersAsync(userId);
//            ViewBag.MyUserId = userId;
//            return View(contacts); // burada artık List<AppUser> geliyor
//        }
//        [HttpGet]
//        public async Task<IActionResult> GetMessages(string receiverId)
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var messages = await _messageService.GetMessagesBetweenAsync(userId, receiverId);
//            ViewBag.MyUserId = userId;
//            return PartialView("_MessageHistoryPartial", messages); // Views/Chat/_MessageHistoryPartial.cshtml
//        }

//        [HttpGet]
//        public async Task<IActionResult> SearchUser(string query)
//        {
//            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            var results = await _userManager.Users
//                .Where(u => u.UserName.Contains(query) && u.Id != currentUserId)
//                .Select(u => new { u.Id, u.UserName })
//                .Take(10)
//                .ToListAsync();

//            return Json(results);
//        }
//    }
//}
