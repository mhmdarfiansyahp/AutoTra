using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class ItemController : Controller
    {
        private readonly Item itmrepositori;
        public ItemController(IConfiguration configuration)
        {
            itmrepositori = new Item(configuration);
        }
        public IActionResult Index()
        {
            ViewData["StdList"] = itmrepositori.getAllStd();
            ViewData["KtgList"] = itmrepositori.getAllKtg();
            return View(itmrepositori.getAllData());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ItemModel itm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    itmrepositori.insertdata(itm);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return View(itm);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ItemModel itmmodel = itmrepositori.getdata(id);
            if (itmmodel == null)
            {
                return NotFound();
            }

            return View(itmmodel);
        }

        [HttpPost]
        public IActionResult Edit(ItemModel itmmodel)
        {
            if (ModelState.IsValid)
            {
                ItemModel newitm = itmrepositori.getdata(itmmodel.id_item);
                if (newitm == null)
                {
                    return NotFound();
                }
                newitm.nama = itmmodel.nama;
                newitm.id_standart = itmmodel.id_standart;
                newitm.id_kategori = itmmodel.id_kategori;
                newitm.metode_inspeksi = itmmodel.metode_inspeksi;
                newitm.status = itmmodel.status;
                itmrepositori.updatedata(newitm);
                TempData["SuccessMessage"] = "Item Inspection data updated successfully.";
                return RedirectToAction("Index");
            }
            return View(itmmodel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = new { success = false, message = "Gagal menghapus item inspeksi." };

            try
            {
                if (id != null)
                {
                    itmrepositori.deletedata(id);
                    response = new { success = true, message = "Item Inspection berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "Item Inspection tidak ditemukan." };

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
