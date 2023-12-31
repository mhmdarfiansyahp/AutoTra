using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class FormController : Controller
    {
        private readonly Form formrepositori;
        public FormController(IConfiguration configuration)
        {
            formrepositori = new Form(configuration);
        }
        [HttpPost]
        public IActionResult Index(string search)
        {
            ViewData["CarData"] = formrepositori.getAllCarIndex();
            return View(formrepositori.getSearchForm(search));
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["CarData"] = formrepositori.getAllCarIndex();
            return View(formrepositori.getAllData());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CarList"] = formrepositori.getAllCar();
            return View();
        }

        [HttpPost]
        public IActionResult Create(FormModel form)
        {
            if (ModelState.IsValid)
            {
                formrepositori.insertdata(form);
                TempData["Success"] = true;
                return RedirectToAction("Index");
            }
            ViewData["CarList"] = formrepositori.getAllCar();
            return View(form);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = new { success = false, message = "Gagal menghapus form inspeksi." };

            try
            {
                if (id != null)
                {
                    formrepositori.deletedata(id);
                    response = new { success = true, message = "Inspection Form berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "Inspection Form tidak ditemukan." };

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
