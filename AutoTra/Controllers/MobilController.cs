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
                    // Panggil insertdata dengan allowNullVin berdasarkan jenis_mobil
                    mobilrepositori.insertdata(mbl, allowNullVin: mbl.jenis_mobil == "Asset");

                    TempData["Success"] = true;
                    return RedirectToAction("Index");
                }
                else
                {
                    // Log ModelState errors
                    foreach (var modelStateKey in ModelState.Keys)
                    {
                        var modelStateVal = ModelState[modelStateKey];
                        if (modelStateVal.Errors.Any())
                        {
                            foreach (var error in modelStateVal.Errors)
                            {
                                Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                            }
                        }
                    }

                    // Return to the view with validation errors
                    return View(mbl);
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine(ex.ToString());

                // You might want to redirect to an error page or handle the exception accordingly
                return View("Error");
            }
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

            if (!string.IsNullOrEmpty(mblmodel.vin) && mblmodel.vin.Length == 17)
            {
                ViewData["VinCharacters"] = mblmodel.vin.ToCharArray();
            }

            return View(mblmodel);
        }

        [HttpPost]
        public IActionResult Edit(MobilModel mblmodel)
        {
            if (ModelState.IsValid) // Pastikan model valid sebelum melanjutkan
            {
                Console.WriteLine(mblmodel.jenis_mobil);

                MobilModel existingMbl = mobilrepositori.getdata(mblmodel.id_mobil);
                if (existingMbl == null)
                {
                    return NotFound();
                }

                // Periksa apakah jenis_mobil bukan 'Asset' sebelum mengupdate vin
                if (mblmodel.jenis_mobil != "Asset")
                {
                    existingMbl.vin = mblmodel.vin;
                }

                existingMbl.jenis_mobil = mblmodel.jenis_mobil;
                existingMbl.nama = mblmodel.nama;
                existingMbl.no_engine = mblmodel.no_engine;
                existingMbl.warna = mblmodel.warna;
                existingMbl.kilometer = mblmodel.kilometer;
                existingMbl.bahan_bakar = mblmodel.bahan_bakar;
                existingMbl.status = mblmodel.status;

                mobilrepositori.updatedata(existingMbl);
                TempData["IsUpdateSuccess"] = true;
                return RedirectToAction("Index");
            }

            return View(mblmodel);
        }

        [HttpPost]
        public IActionResult Delete(string id_mobil)
        {
            var response = new { success = false, message = "Gagal menghapus mobil." };
            Console.WriteLine("id " + id_mobil);
            Console.WriteLine("try");

            try
            {
                if (id_mobil != "")
                {
                    Console.WriteLine("if");
                    Console.WriteLine("if" + id_mobil);
                    mobilrepositori.deletedata(id_mobil);
                    response = new { success = true, message = "Car successfully deleted." };
                }
                else
                {
                    response = new { success = false, message = "Car not found." };
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