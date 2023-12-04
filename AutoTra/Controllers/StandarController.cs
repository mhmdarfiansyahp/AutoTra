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
            try
            {
                if (ModelState.IsValid)
                {
                    stdrepositori.insertdata(std);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
            if (ModelState.IsValid)
            {
                StandarModel newstd = stdrepositori.getdata(stdmodel.id);
                if (newstd == null)
                {
                    return NotFound();
                }
                newstd.nama = stdmodel.nama;
                stdrepositori.updatedata(newstd);
                TempData["SuccessMessage"] = "Mobil berhasil diupdate.";
                return RedirectToAction("Index");
            }
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
