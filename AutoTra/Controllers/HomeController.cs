using AutoTra.Models;
using Microsoft.AspNetCore.Authorization;
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
                Console.WriteLine("adm");

                // Admin login logic
                if (adminModel.status == 0)
                {
                    TempData["Notification"] = "Username atau Password tidak Valid.";
                    return RedirectToAction("Index", "Home");
                } 

                string serializedModelFromDb = JsonConvert.SerializeObject(adminModel);
                HttpContext.Session.SetString("Identity", serializedModelFromDb);
                HttpContext.Session.SetString("Role", adminModel.peran);
                HttpContext.Session.SetString("nama", adminModel.nama);

                return RedirectToAction("Index", "DashboardAdmin");

            }
            else if (i==2)
            {
                // Dosen login logic
         
                string serializedModelFromDb = JsonConvert.SerializeObject(dosenModel);
                HttpContext.Session.SetString("Identity", serializedModelFromDb);
                HttpContext.Session.SetString("Identity", serializedModelFromDb);
                HttpContext.Session.SetString("Role", "Lecturer");
                HttpContext.Session.SetString("nama", dosenModel.nama);
                HttpContext.Session.SetString("npk", dosenModel.npk);


                return RedirectToAction("Index", "DashboardDosen"); // Ensure the correct area is set

            }
            else if(i==3)
            {
                string serializedModelFromDb = JsonConvert.SerializeObject(picmodel);
                HttpContext.Session.SetString("Identity", serializedModelFromDb);
                HttpContext.Session.SetString("Identity", serializedModelFromDb);
                HttpContext.Session.SetString("Role", "PIC");
                HttpContext.Session.SetString("nim", picmodel.nim); // Set the "nim" value in the session
                HttpContext.Session.SetString("nama", picmodel.nama);

                return RedirectToAction("Index", "DashboardPIC");
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
