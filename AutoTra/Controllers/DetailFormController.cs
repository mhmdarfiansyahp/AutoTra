using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace AutoTra.Controllers
{
    public class DetailFormController : Controller
    {
        private readonly DetailForm dtlformrepositori;
        public DetailFormController(IConfiguration configuration)
        {
            dtlformrepositori = new DetailForm(configuration);
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {

            DetailFormModel carmodel = dtlformrepositori.getdata(id);
            ViewData["DataItem"] = dtlformrepositori.getDataItem();
            ViewData["DataDetail"] = dtlformrepositori.getAllData(id);
            if (carmodel == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(carmodel.vin) && carmodel.vin.Length == 17)
            {
                ViewData["VinCharacters"] = carmodel.vin.ToCharArray();
            }

            return View(carmodel);
        }
        [HttpPost]
        public IActionResult Create(DetailFormModel form)
        {
            if (ModelState.IsValid)
            {
                dtlformrepositori.insertdata(form);
                int? newId = form.id_form;
                return RedirectToAction("Detail", new { id = newId });
            }
            return View(form);
        }
        public IActionResult Delete(int id)
        {
            var response = new { success = false, message = "Gagal menghapus item form inspeksi." };

            try
            {
                if (id != null)
                {
                    dtlformrepositori.deletedata(id);
                    response = new { success = true, message = "Inspection Item for Form berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "Inspection Item for Form tidak ditemukan." };

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
