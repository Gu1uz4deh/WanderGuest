using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WanderQuest.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}
