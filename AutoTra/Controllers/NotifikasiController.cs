using AutoTra.Model;
using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class NotifikasiController : Controller
    {
        private readonly Notifikasi notifikasirepositori;
        private readonly Dosen dsnrepository;
        private readonly PIC picrepository;
        ResponseModel response = new ResponseModel();

        public NotifikasiController(IConfiguration configuration)
        {
            notifikasirepositori = new Notifikasi(configuration);
            dsnrepository = new Dosen(configuration);
            picrepository = new PIC(configuration);
        }
        public IActionResult Index()
        {
            var notifikasiList = notifikasirepositori.GetNotifikasiListFromPgnUnitPraktek();
            var dsn = dsnrepository.getAllData();
            var pic = picrepository.getAllData();

            var dsndictinory = dsn.ToDictionary(d => d.npk, d => d.nama);
            var Picdictionary = pic.ToDictionary(p => p.nim, p => p.nama);

            ViewBag.dsndictinary = dsndictinory;
            ViewBag.picdictinary = Picdictionary;
            return View(notifikasiList);
        }

        [HttpGet]
        public IActionResult GetUnreadNotificationCount()
        {
            var notifikasiList = notifikasirepositori.GetNotifikasiListFromPgnUnitPraktek();
            var unreadCount = notifikasiList.Count(n => n.status != 0 && !n.IsRead);
            return Json(new { unreadCount });
        }

    }
}
