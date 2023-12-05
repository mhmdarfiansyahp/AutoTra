using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace AutoTra.Controllers
{
    public class HomeController : Controller
    {
        private readonly Admin adminrepositori;
        private readonly Dosen dsnrepositori;
        private readonly PIC picrepositori;

        public HomeController(IConfiguration configuration)
        {
            adminrepositori = new Admin(configuration);
            dsnrepositori = new Dosen(configuration);
            picrepositori = new PIC(configuration);
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            int i = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Username dan password diperlukan.";
                return RedirectToAction("Index", "Home");
            }

            AdminModel adminModel = adminrepositori.getDataByUsername_Password(username, password);
            DosenModel dosenModel = dsnrepositori.getDataByUsername_Password(username, password);
            PICModel picmodel = picrepositori.getDataByUsername_Password(username, password);
            Console.WriteLine(dosenModel.npk);
            if (adminModel != null && adminModel.npk != null)
            {
                i = 1;
            } else if(dosenModel != null && dosenModel.npk != null)
            {
                i = 2;
            } else if(picmodel != null && picmodel.nim != null)
            {
                i = 3;
            }
            
            if (i==1)
            {
                Console.WriteLine(adminModel.ToString());

                // Admin login logic
                if (adminModel.status == 0)
                {
                    TempData["Notification"] = "Akun tidak aktif. Harap hubungi administrator.";
                    return RedirectToAction("Index", "Home");
                } 

                string serializedModelFromDb = JsonConvert.SerializeObject(adminModel);
                HttpContext.Session.SetString("Identity", serializedModelFromDb);

                return RedirectToAction("Index", "Dashboard");

            }
            else if (i==2)
            {
                // Dosen login logic
                Console.WriteLine(adminModel.ToString());
         
                string serializedModelFromDb = JsonConvert.SerializeObject(dosenModel);
                HttpContext.Session.SetString("Identity", serializedModelFromDb);

                return RedirectToAction("Index", "DashboardDosen", new { area = "" }); // Ensure the correct area is set

            }
            else if(i==3)
            {
                string serializedModelFromDb = JsonConvert.SerializeObject(picmodel);
                HttpContext.Session.SetString("Identity", serializedModelFromDb);

                return RedirectToAction("Index", "DashboardPIC");
            }
            else if (string.Equals(username, "superadmin", StringComparison.OrdinalIgnoreCase) && string.Equals(password, "123"))
            {
                // Static credentials matched
                string serializedModel = JsonConvert.SerializeObject(new AdminModel
                {
                    npk = "superadmin",
                    nama = "Super Admin",
                    username = "superadmin",
                    password = "123",
                    peran = "Superadmin",
                    status = 1
                });
                HttpContext.Session.SetString("Identity", serializedModel);

                // Redirect to the DashboardAdmin/Index action
                return RedirectToAction("Index", "DashboardAdmin");
            }
            else
            {
                TempData["Notification"] = "Akun tidak aktif. Harap hubungi administrator.";
                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
