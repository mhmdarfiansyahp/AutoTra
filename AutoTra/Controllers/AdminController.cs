using Microsoft.AspNetCore.Mvc;
using AutoTra.Models;

namespace AutoTra.Controllers
{
    public class AdminController : Controller
    {
        private readonly Admin adminrepositori;

        public AdminController(IConfiguration configuration)
        {
            adminrepositori = new Admin(configuration);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(adminrepositori.getAllData());
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (search != null)
            {
                return View(adminrepositori.getSearch(search));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AdminModel adm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the NPK or username already exists
                    if (adminrepositori.IsNpkExists(adm.npk) || adminrepositori.IsUsernameExists(adm.username, adm.npk))
                    {
                        if (adminrepositori.IsNpkExists(adm.npk))
                        {
                            ModelState.AddModelError("npk", "NPK already exists. Please choose a different one.");
                        }

                        if (adminrepositori.IsUsernameExists(adm.username, adm.npk))
                        {
                            ModelState.AddModelError("username", "Username already exists. Please choose a different one.");
                        }
                    }

                    // If both NPK and username are unique, insert data
                    if (ModelState.IsValid)
                    {
                        adminrepositori.insertData(adm);
                        TempData["SuccessMessage"] = "Data added successfully";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return View(adm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            AdminModel filmmodel = adminrepositori.getData(id);
            if (filmmodel == null)
            {
                return NotFound();
            }

            return View(filmmodel);
        }


        [HttpPost]
        public IActionResult Edit(AdminModel admmodel)
        {
            if (ModelState.IsValid)
            {
                // Pemeriksaan apakah username sudah ada
                if (adminrepositori.IsUsernameExists(admmodel.username, admmodel.npk))
                {
                    ModelState.AddModelError("username", "Username already exists. Please choose a different one.");
                    return View(admmodel);
                }

                if (int.TryParse(admmodel.npk, out int Npk))
                {
                    AdminModel newadm = adminrepositori.getData(Npk);
                    if (newadm == null)
                    {
                        return NotFound();
                    }

                    newadm.nama = admmodel.nama;
                    newadm.username = admmodel.username;
                    newadm.password = admmodel.password;
                    newadm.peran = admmodel.peran;
                    newadm.status = admmodel.status;
                    adminrepositori.updateData(newadm);
                    TempData["SuccessMessage"] = "Admin updated successfully.";
                    return RedirectToAction("Index");
                }
            }

            return View(admmodel);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var response = new { success = false, message = "Gagal menghapus admin." };

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    adminrepositori.deleteData(id);
                    response = new { success = true, message = "Admin successfully changed his status." };
                }
                else
                {
                    response = new { success = false, message = "Admin tidak ditemukan." };
                }
            }
            catch (Exception ex)
            {
                response = new { success = false, message = ex.Message };
            }
            return Json(response);
        }

    }
}
