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

        public DaftarPengajuanController(IConfiguration configuration)
        {
            Dpengajuanrepositori = new DaftarPengajuan(configuration);
            mobilrepository = new Mobil(configuration);
            dsnrepository = new Dosen(configuration);
            picrepository = new PIC(configuration);
        }
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

    }
}
