using AutoTra.Model;
using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{
    public class Notifikasi2Controller : Controller
    {
        private readonly Notifikasi2 notifikasirepositori;
        private readonly Dosen dsnrepository;
        private readonly PIC picrepository;
        ResponseModel response = new ResponseModel();

        public Notifikasi2Controller(IConfiguration configuration)
        {
            notifikasirepositori = new Notifikasi2(configuration);
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
            var notifikasi2List = notifikasiList.Select(n => new Notifikasi2Model
            {
                nim = n.nim,
                deskripsi = n.deskripsi,
                tanggl_pengajuan = n.tanggl_pengajuan,
                status = n.status
            }).ToList();
            return View(notifikasi2List);
        }

        [HttpGet]
        public IActionResult GetUnreadNotificationCount()
        {
            var notifikasiList = notifikasirepositori.GetNotifikasiListFromPgnUnitPraktek();
            var unreadCount = notifikasiList.Count(n => !n.IsRead && (n.status == 0 || n.status == 3));
            return Json(new { unreadCount });
        }
    }
}
