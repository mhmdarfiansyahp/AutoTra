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
            var mobilData = mobilrepository.getActiveCar();
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
                    TempData["SuccessMessage"] = "Data added successfully";
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
        public IActionResult FirstCheck(int? id)
        {
            ViewData["DataForm"] = pengajuanrepositori.getForm(id);
            ViewData["DataItem"] = pengajuanrepositori.getDataItem();
            PengajuanKendaraanModel pengajuan = pengajuanrepositori.getPemeriksaan(id);
            ViewBag.id = pengajuan.id_pemeriksaan;
            ViewBag.id_pengajuan = id;
            return View();
        }

        [HttpPost]
        public IActionResult FirstCheck([FromBody] List<PengajuanKendaraanModel> pengajuanmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pengajuanrepositori.insertdetailpemeriksaan(pengajuanmodel);
                    TempData["SuccessMessage"] = "Status of Submissions was changed!";
                    return Ok("Data berhasil diproses");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Terjadi kesalahan: {ex.Message}");
            }
            return View(pengajuanmodel);
        }
        [HttpGet]
        public IActionResult FinalCheck(int? id)
        {
            ViewData["DataForm"] = pengajuanrepositori.getForm(id);
            ViewData["DataItem"] = pengajuanrepositori.getDataItem();
            ViewData["DataPengajuan"] = pengajuanrepositori.getPengajuan(id);
            PengajuanKendaraanModel pengajuan = pengajuanrepositori.getPemeriksaan1(id);
            ViewBag.id = pengajuan.id_pemeriksaan;
            ViewBag.id_pengajuan = id;
            return View();
        }

        [HttpPost]
        public IActionResult FinalCheck([FromBody] List<PengajuanKendaraanModel> pengajuanmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pengajuanrepositori.insertdetailpemeriksaanfinal(pengajuanmodel);
                    TempData["SuccessMessage"] = "Status of Submissions was changed!";
                    return Ok("Data berhasil diproses");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Terjadi kesalahan: {ex.Message}");
            }
            return View(pengajuanmodel);
        }

        [HttpPost]
        public IActionResult Laporan(string search)
        {
            try
            {
                var mobil = mobilrepository.getAllData();
                var mobildictionary = mobil.ToDictionary(m => m.id_mobil, m => m.nama);
                ViewBag.Mobildictinary = mobildictionary;
                List<PengajuanKendaraanModel> data = pengajuanrepositori.getSearchLaporan(search);
                if (data == null)
                {
                    return NotFound();
                }

                ViewData["DataPIC"] = pengajuanrepositori.getAllPIC();
                return View(data);
            }
            catch (Exception ex)
            {
                // Tangani pengecualian jika terjadi kesalahan
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Laporan()
        {
            try
            {
                var mobil = mobilrepository.getAllData();
                var mobildictionary = mobil.ToDictionary(m => m.id_mobil, m => m.nama);
                ViewBag.Mobildictinary = mobildictionary;
                List<PengajuanKendaraanModel> data = pengajuanrepositori.getdetailpemeriksaan();
                if (data == null)
                {
                    return NotFound();
                }

                ViewData["DataPIC"] = pengajuanrepositori.getAllPIC();
                return View(data);
            }
            catch (Exception ex)
            {
                // Tangani pengecualian jika terjadi kesalahan
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult LaporanDetail(int id)
        {
            var dsn = dsnrepository.getAllData();
            var dsndictinory = dsn.ToDictionary(d => d.npk, d => d.nama);
            ViewBag.dsndictinary = dsndictinory;
            ViewData["DataPengajuan"] = pengajuanrepositori.getdetailpemeriksaan2(id);
            ViewData["DataItem"] = pengajuanrepositori.getDataItem();
            ViewData["DataDetail"] = pengajuanrepositori.getDetailPemeriksaan(id);
            ViewData["DataMobil"] = pengajuanrepositori.getAllCarData();
            ViewData["DataPIC"] = pengajuanrepositori.getAllPIC();
            return View();
        }
    }
}




