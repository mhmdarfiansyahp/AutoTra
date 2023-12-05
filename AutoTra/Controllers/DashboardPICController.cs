using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class DashboardPICController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
