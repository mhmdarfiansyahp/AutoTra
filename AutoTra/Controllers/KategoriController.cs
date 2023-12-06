using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.SqlClient;

namespace AutoTra.Controllers
{
    public class KategoriController : Controller
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        private readonly Kategori ktgrepositori;
        public KategoriController(IConfiguration configuration)
        {
            ktgrepositori = new Kategori(configuration);
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
                string query = "SELECT COUNT(*) FROM dbo.Kt_Pemeriksaan WHERE nama = @Nama AND status != 0";
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
            return View(ktgrepositori.getAllData());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(KategoriModel ktg)
        {

                if (ModelState.IsValid)
                {
                    ktgrepositori.insertdata(ktg);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
            
            TempData["ErrorMessage"] = " Description of Category Inspection was added.";
            return View(ktg);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            KategoriModel ktgmodel = ktgrepositori.getdata(id);
            if (ktgmodel == null)
            {
                return NotFound();
            }

            return View(ktgmodel);
        }

        [HttpPost]
        public IActionResult Edit(KategoriModel ktgmodel)
        {

                if (ModelState.IsValid)
                {
                    KategoriModel newktg = ktgrepositori.getdata(ktgmodel.id);
                    if (newktg == null)
                    {
                        return NotFound();
                    }
                    newktg.nama = ktgmodel.nama;
                    ktgrepositori.updatedata(newktg);
                    TempData["SuccessMessage"] = "Category Inspection updated successfully.";
                    return RedirectToAction("Index");
                }
            
            TempData["ErrorMessage"] = " Description of Category Inspection was added.";
            return View(ktgmodel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = new { success = false, message = "Gagal menghapus kategori inspeksi." };

            try
            {
                if (id != null)
                {
                    ktgrepositori.deletedata(id);
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
