using AutoTra.Model;
using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class DaftarPengajuanController : Controller
    {
        private readonly DaftarPengajuan Dpengajuanrepositori;
        private readonly Mobil mobilrepository;
        private readonly Dosen dsnrepository;
        private readonly PIC picrepository;
        ResponseModel response = new ResponseModel();

        public DaftarPengajuanController(IConfiguration configuration)
        {
            Dpengajuanrepositori = new DaftarPengajuan(configuration);
            mobilrepository = new Mobil(configuration);
            dsnrepository = new Dosen(configuration);
            picrepository = new PIC(configuration);
        }
        [HttpGet]
        public IActionResult Index()
        {
            // Get the current logged-in username from session
            var npk = HttpContext.Session.GetString("npk");

            // Check if the npk is available
            if (string.IsNullOrEmpty(npk))
            {
                TempData["ErrorMessage"] = "Mohon login terlebih dahulu.";
                return RedirectToAction("Index", "Home");
            }

            var pengajuan = Dpengajuanrepositori.getAllData(npk);
            var mobil = mobilrepository.getAllData();
            var dsn = dsnrepository.getAllData();
            var pic = picrepository.getAllData();

            var dsndictinory = dsn.ToDictionary(d => d.npk, d => d.nama);
            var mobildictionary = mobil.ToDictionary(m => m.id_mobil, m => m.nama);
            var Picdictionary = pic.ToDictionary(p => p.nim, p =>  p.nama);

            ViewBag.dsndictinary = dsndictinory;
            ViewBag.Mobildictinary = mobildictionary;
            ViewBag.picdictinary = Picdictionary;
            return View(pengajuan);
        }
        [HttpPost]
        public IActionResult Approve1(int approvalStatus, int id_pengajuan, int id_mobil, DateTime tanggal_pemeriksaan, string skala, string NIM, int status_pemeriksaan)
        {
            try
            {
                // Lakukan validasi jika diperlukan
                if (approvalStatus != 0) 
                {
                    // Lakukan sesuatu dengan data yang diterima dari formulir
                    if (approvalStatus == 2 || approvalStatus == 1) // Mengubah dua blok if menjadi satu dengan || (atau)
                    {
                        Dpengajuanrepositori.approval1(approvalStatus, id_pengajuan, id_mobil, tanggal_pemeriksaan, skala, NIM, status_pemeriksaan);
                    }
                    else
                    {
                        // Jika approvalStatus bukan 1 atau 2
                        return Json(new { success = false, message = "Nilai approvalStatus tidak valid." });
                    }

                    TempData["SuccessMessage"] = "Status of Submissions was changed!";
                    return Json(new { success = true, message = "Data berhasil diproses." });
                }
                else
                {
                    // Jika data tidak valid atau kosong
                    return Json(new { success = false, message = "Data tidak valid." });
                }
            }
            catch (Exception ex)
            {
                // Tangani pengecualian jika terjadi kesalahan
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }
        [HttpPost]
        public IActionResult Approve2(int approvalStatus, int id_pengajuan, int id_mobil, DateTime tanggal_pemeriksaan, string skala, string NIM, int status_pemeriksaan)
        {
            try
            {
                // Lakukan validasi jika diperlukan
                if (approvalStatus != 0)
                {
                    // Lakukan sesuatu dengan data yang diterima dari formulir
                    if (approvalStatus == 4 || approvalStatus == 1) // Mengubah dua blok if menjadi satu dengan || (atau)
                    {
                        Dpengajuanrepositori.approval2(approvalStatus, id_pengajuan, id_mobil, tanggal_pemeriksaan, skala, NIM, status_pemeriksaan);
                    }
                    else
                    {
                        // Jika approvalStatus bukan 1 atau 2
                        return Json(new { success = false, message = "Nilai approvalStatus tidak valid." });
                    }

                    TempData["SuccessMessage"] = "Status of Submissions was changed!";
                    return Json(new { success = true, message = "Data berhasil diproses." });
                }
                else
                {
                    // Jika data tidak valid atau kosong
                    return Json(new { success = false, message = "Data tidak valid." });
                }
            }
            catch (Exception ex)
            {
                // Tangani pengecualian jika terjadi kesalahan
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Approval1(int id)
        {
            DaftarPengajuanModel dpengajuan = Dpengajuanrepositori.getdetailacc1(id);
            if (dpengajuan == null)
            {
                return NotFound();
            }

            return Json(dpengajuan);
        }
        [HttpGet]
        public IActionResult Approval2(int id)
        {
            try
            {
                List<DaftarPengajuanModel> data = Dpengajuanrepositori.getdetailacc2(id);
                if (data == null)
                {
                    return NotFound();
                }

                return Json(data);
            }
            catch (Exception ex)
            {
                // Tangani pengecualian jika terjadi kesalahan
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

    }
}
