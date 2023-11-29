using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    
    public class DosenController : Controller
    {
        private readonly Dosen dosenrepositori;
        public DosenController(IConfiguration configuration)
        {
            dosenrepositori = new Dosen(configuration);
        }
        public IActionResult Index()
        {
            return View(dosenrepositori.getAllData());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DosenModel adm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the username already exists
                    if (dosenrepositori.IsUsernameExists(adm.username))
                    {
                        ModelState.AddModelError("username", "Username already exists. Please choose a different one.");
                        return View(adm);
                    }
                    dosenrepositori.insertdata(adm);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
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
            DosenModel filmmodel = dosenrepositori.getdata(id);
            if (filmmodel == null)
            {
                return NotFound();
            }

            return View(filmmodel);
        }

        [HttpPost]
        public IActionResult Edit(DosenModel dsnmodel)
        {
            if (ModelState.IsValid)
            {
                if (int.TryParse(dsnmodel.npk, out int Npk))
                {
                    DosenModel newadm = dosenrepositori.getdata(Npk);
                    if (newadm == null)
                    {
                        return NotFound();
                    }

                    newadm.nama = dsnmodel.nama;
                    newadm.username = dsnmodel.username;
                    newadm.password = dsnmodel.password;
                    newadm.peran = dsnmodel.peran;
                    newadm.status = dsnmodel.status;
                    dosenrepositori.updatedata(newadm);
                    TempData["SuccessMessage"] = "Dosen berhasil diupdate.";
                    return RedirectToAction("Index");
                }
            }
            return View(dsnmodel);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var response = new { success = false, message = "Gagal menghapus admin." };

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    dosenrepositori.deletedata(id);
                    response = new { success = true, message = "Dosen berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "Dosen tidak ditemukan." };
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
