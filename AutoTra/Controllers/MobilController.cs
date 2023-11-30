using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{

    public class MobilController : Controller
    {
        private readonly Mobil mobilrepositori;
        public MobilController(IConfiguration configuration)
        {
            mobilrepositori = new Mobil(configuration);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(mobilrepositori.getAllData());
        }
        [HttpPost]
        public IActionResult Index(string search)
        {
            if (search != null)
            {
                return View(mobilrepositori.getSearch(search));
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
        public IActionResult Create(MobilModel mbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    mobilrepositori.insertdata(mbl);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return View(mbl);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            MobilModel mblmodel = mobilrepositori.getdata(id);
            if (mblmodel == null)
            {
                return NotFound();
            }
            Console.WriteLine(mblmodel.id_mobil);

            return View(mblmodel);
        }

        [HttpPost]
        public IActionResult Edit(MobilModel mblmodel)
        {
            if (true)
            {
                Console.WriteLine(mblmodel.jenis_mobil);
                if (int.TryParse(mblmodel.id_mobil, out int id_mobil))
                {
                    MobilModel newmbl = mobilrepositori.getdata(id_mobil);
                    if (newmbl == null)
                    {
                        return NotFound();
                    }

                    newmbl.jenis_mobil = mblmodel.jenis_mobil;
                    newmbl.nama = mblmodel.nama;
                    newmbl.vin = mblmodel.vin;
                    newmbl.no_engine = mblmodel.no_engine;
                    newmbl.warna = mblmodel.warna;
                    newmbl.kilometer = mblmodel.kilometer;
                    newmbl.bahan_bakar = mblmodel.bahan_bakar;
                    newmbl.status = mblmodel.status;
                    mobilrepositori.updatedata(newmbl);
                    TempData["SuccessMessage"] = "Mobil berhasil diupdate.";
                    return RedirectToAction("Index");
                }
            }
            return View(mblmodel);
        }

        [HttpPost]
        public IActionResult Delete(string id_mobil)
        {
            var response = new { success = false, message = "Gagal menghapus mobil." };
            Console.WriteLine("id "+id_mobil);
            Console.WriteLine("try");

            try
            {
                if (id_mobil != "")
                {
                    Console.WriteLine("if");
                    Console.WriteLine("if" + id_mobil);
                    mobilrepositori.deletedata(id_mobil);
                    response = new { success = true, message = "Mobil berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "Mobil tidak ditemukan." };
                    Console.WriteLine("else");
                }
            }
            catch (Exception ex)
            {
                response = new { success = false, message = ex.Message };
                Console.WriteLine("catch");
            }
            return Json(response);
        }
    }
}
