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
            return View(itmrepositori.getAllData());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ItemModel itm)
        {

            if (ModelState.IsValid)
            {
                itmrepositori.insertdata(itm);
                TempData["Success"] = true;
                return RedirectToAction("Index");
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
                newitm.item_pemeriksaan = itmmodel.item_pemeriksaan;
                newitm.kategori_pemeriksaan = itmmodel.kategori_pemeriksaan;
                newitm.standart_pemeriksaan = itmmodel.standart_pemeriksaan;
                newitm.metode_pemeriksaan = itmmodel.metode_pemeriksaan;
                newitm.status = itmmodel.status;
                itmrepositori.updatedata(newitm);
                TempData["IsUpdateSuccess"] = true;
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
