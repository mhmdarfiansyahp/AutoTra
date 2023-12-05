using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoTra.Controllers
{
    public class DashboardDosenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
