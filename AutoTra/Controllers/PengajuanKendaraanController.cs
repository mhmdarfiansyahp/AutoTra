using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoTra.Controllers
{
    public class PengajuanKendaraanController : Controller
    {
        private readonly PengajuanKendaraan pengajuanrepositori;
        private readonly Mobil mobilrepository;
        private readonly Dosen dsnrepository;
        public PengajuanKendaraanController(IConfiguration configuration)
        {
            pengajuanrepositori = new PengajuanKendaraan(configuration);
            mobilrepository = new Mobil(configuration);
            dsnrepository = new Dosen(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var pengajuan = pengajuanrepositori.getAllData();
            var mobil = mobilrepository.getAllData();
            var dsn = dsnrepository.getAllData();

            var dsndictinory = dsn.ToDictionary(d => d.npk, d => d.nama);
            var mobildictionary = mobil.ToDictionary(m => m.id_mobil, m => m.nama);

            ViewBag.dsndictinary = dsndictinory;
            ViewBag.Mobildictinary = mobildictionary;

            return View(pengajuan);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var mobilData = mobilrepository.getAllData();
            var DsnData = dsnrepository.getAllData();

            ViewBag.Dosen = new SelectList(DsnData, "npk", "nama");
            ViewBag.Mobil = new SelectList(mobilData, "id_mobil", "nama");

            // Check the user's role
            var userRole = HttpContext.Session.GetString("Role");

            string userNim = string.Empty;
            string userNama = string.Empty;

            if (userRole == "PIC")
            {
                // If the user is a PIC, concatenate "nim" and "nama" values
                userNama = HttpContext.Session.GetString("nama");
                userNim = HttpContext.Session.GetString("nim");
                ViewBag.NimNama = $"{userNim} - {userNama}";
            }
            else
            {
                // If the user is not a PIC, let the "nim" field be editable
                ViewBag.NimNama = string.Empty;
            }

            ViewBag.Nim = userNim;
            ViewBag.Nama = userNama;

            var model = new PengajuanKendaraanModel
            {
                tanggl_pengajuan = DateTime.Today,
                nim = userNim // Set the nim property based on user role
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PengajuanKendaraanModel mbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pengajuanrepositori.insertdata(mbl);
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
    }
}
