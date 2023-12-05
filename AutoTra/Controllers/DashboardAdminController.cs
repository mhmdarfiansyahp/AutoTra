using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class DashboardAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
