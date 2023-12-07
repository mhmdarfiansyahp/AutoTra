using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.SqlClient;

namespace AutoTra.Controllers
{
    public class StandarController : Controller
    {
        private readonly Standar stdrepositori;
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        public StandarController(IConfiguration configuration)
        {
            stdrepositori = new Standar(configuration);
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        private string GetConnectionString()
        {
            // Fungsi ini mengembalikan string koneksi ke database SQL Server
            return _connectionString; // Ganti dengan string koneksi Anda
        }
        [HttpGet]
        public IActionResult CheckNamaExist(string nama)
        {
            bool exists = IsNamaExistsInDatabase(nama);
            return Json(new { exists = exists });
        }

        private bool IsNamaExistsInDatabase(string nama)
        {
            bool exists = false;
            string _connection = GetConnectionString();

            using (SqlConnection connection = new SqlConnection(_connection))
            {
                string query = "SELECT COUNT(*) FROM dbo.Std_Pemeriksaan WHERE nama = @Nama AND status != 0";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nama", nama);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                exists = count > 0;
            }

            return exists;
        }
        public IActionResult Index()
        {
            return View(stdrepositori.getAllData());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StandarModel std)
        {
                if (ModelState.IsValid)
                {
                    stdrepositori.insertdata(std);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }

            TempData["ErrorMessage"] = " Description of Standart Inspection was added.";
            return View(std);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            StandarModel stdmodel = stdrepositori.getdata(id);
            if (stdmodel == null)
            {
                return NotFound();
            }

            return View(stdmodel);
        }

        [HttpPost]
        public IActionResult Edit(StandarModel stdmodel)
        {

                if (ModelState.IsValid)
                {
                    StandarModel newstd = stdrepositori.getdata(stdmodel.id);
                    if (newstd == null)
                    {
                        return NotFound();
                    }
                    newstd.id = stdmodel.id;
                    newstd.nama = stdmodel.nama;
                    stdrepositori.updatedata(newstd);
                    TempData["SuccessMessage"] = "Data Standar Inspeksi berhasil diupdate.";
                    return RedirectToAction("Index");
                }
            
            TempData["ErrorMessage"] = " Description of Standart Inspection was added.";
            return View(stdmodel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = new { success = false, message = "Gagal menghapus standar inspeksi." };

            try
            {
                if (id != null)
                {
                    stdrepositori.deletedata(id);
                    response = new { success = true, message = "Inspection Standart berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "Inspection Standart tidak ditemukan." };

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
