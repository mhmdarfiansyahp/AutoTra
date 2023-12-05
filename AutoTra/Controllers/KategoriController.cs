using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class KategoriController : Controller
    {
        private readonly Kategori ktgrepositori;
        public KategoriController(IConfiguration configuration)
        {
            ktgrepositori = new Kategori(configuration);
        }
        public IActionResult Index()
        {
            return View(ktgrepositori.getAllData());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(KategoriModel ktg)
        {
            KategoriModel newktg = ktgrepositori.getname(ktg.nama);
            if (newktg == null)
            {
                if (ModelState.IsValid)
                {
                    ktgrepositori.insertdata(ktg);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
            }
            TempData["ErrorMessage"] = " Description of Category Inspection was added.";
            return View(ktg);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            KategoriModel ktgmodel = ktgrepositori.getdata(id);
            if (ktgmodel == null)
            {
                return NotFound();
            }

            return View(ktgmodel);
        }

        [HttpPost]
        public IActionResult Edit(KategoriModel ktgmodel)
        {
            KategoriModel newktg1 = ktgrepositori.getname(ktgmodel.nama);
            if (newktg1 == null)
            {
                if (ModelState.IsValid)
                {
                    KategoriModel newktg = ktgrepositori.getdata(ktgmodel.id);
                    if (newktg == null)
                    {
                        return NotFound();
                    }
                    newktg.nama = ktgmodel.nama;
                    ktgrepositori.updatedata(newktg);
                    TempData["SuccessMessage"] = "Category Inspection updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            TempData["ErrorMessage"] = " Description of Category Inspection was added.";
            return View(ktgmodel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = new { success = false, message = "Gagal menghapus kategori inspeksi." };

            try
            {
                if (id != null)
                {
                    ktgrepositori.deletedata(id);
                    response = new { success = true, message = "Inspection Standart berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "Inspection Standart tidak ditemukan." };

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
