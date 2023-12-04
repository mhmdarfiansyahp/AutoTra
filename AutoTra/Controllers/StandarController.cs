using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class StandarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
