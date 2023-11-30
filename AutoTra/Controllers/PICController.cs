using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;

namespace AutoTra.Controllers
{
    public class PICController : Controller
    {
        private readonly PIC picrepositori;
        public PICController(IConfiguration configuration)
        {
            picrepositori = new PIC(configuration);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(picrepositori.getAllData());
        }
        [HttpPost]
        public IActionResult Index(string search)
        {
            if (search != null)
            {
                return View(picrepositori.getSearch(search));
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
        public IActionResult Create(PICModel pic)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the username already exists
                    if (picrepositori.IsUsernameExists(pic.username))
                    {
                        ModelState.AddModelError("username", "Username already exists. Please choose a different one.");
                        return View(pic);
                    }
                    picrepositori.insertData(pic);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return View(pic);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            PICModel filmmodel = picrepositori.getData(id);
            if (filmmodel == null)
            {
                return NotFound();
            }

            return View(filmmodel);
        }

        [HttpPost]
        public IActionResult Edit(PICModel picmodel)
        {
            if (ModelState.IsValid)
            {
                    PICModel newadm = picrepositori.getData(picmodel.nim);
                    if (newadm == null)
                    {
                        return NotFound();
                    }

                    newadm.nama = picmodel.nama;
                    newadm.username = picmodel.username;
                    newadm.password = picmodel.password;
                    newadm.peran = picmodel.peran;
                    picrepositori.updateData(newadm);
                    TempData["SuccessMessage"] = "PIC berhasil diupdate.";
                    return RedirectToAction("Index");
                }
            return View(picmodel);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var response = new { success = false, message = "Gagal menghapus PIC." };

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    picrepositori.deleteData(id);
                    response = new { success = true, message = "PIC berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "PIC tidak ditemukan." };
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
