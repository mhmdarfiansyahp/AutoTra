using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class StandarController : Controller
    {
        private readonly Standar stdrepositori;
        public StandarController(IConfiguration configuration)
        {
            stdrepositori = new Standar(configuration);
        }
        public IActionResult Index()
        {
            return View(stdrepositori.getAllData());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StandarModel std)
        {
            StandarModel newstd = stdrepositori.getname(std.nama);
            if (newstd == null)
            {
                if (ModelState.IsValid)
                {
                    stdrepositori.insertdata(std);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
            }
            TempData["ErrorMessage"] = " Description of Standart Inspection was added.";
            return View(std);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            StandarModel stdmodel = stdrepositori.getdata(id);
            if (stdmodel == null)
            {
                return NotFound();
            }

            return View(stdmodel);
        }

        [HttpPost]
        public IActionResult Edit(StandarModel stdmodel)
        {
            StandarModel newstd2 = stdrepositori.getname(stdmodel.nama);
            if (newstd2 == null)
            {
                if (ModelState.IsValid)
                {
                    StandarModel newstd = stdrepositori.getdata(stdmodel.id);
                    if (newstd == null)
                    {
                        return NotFound();
                    }
                    newstd.id = stdmodel.id;
                    newstd.nama = stdmodel.nama;
                    stdrepositori.updatedata(newstd);
                    TempData["SuccessMessage"] = "Data Standar Inspeksi berhasil diupdate.";
                    return RedirectToAction("Index");
                }
            }
            TempData["ErrorMessage"] = " Description of Standart Inspection was added.";
            return View(stdmodel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = new { success = false, message = "Gagal menghapus standar inspeksi." };

            try
            {
                if (id != null)
                {
                    stdrepositori.deletedata(id);
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
