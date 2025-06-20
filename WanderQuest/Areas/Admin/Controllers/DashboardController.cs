using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WanderQuest.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}
