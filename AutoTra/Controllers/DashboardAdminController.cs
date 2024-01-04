using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace AutoTra.Controllers
{
    public class DashboardAdminController : Controller
    {
        private readonly string _connectionString;

        public DashboardAdminController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            var dosenCount = GetDosenCount();
            var picCount = GetPICCount();
            var carGasolineCount = GetCarGasolineCount();
            var carElectricCount = GetCarElectricCount();

            ViewBag.DosenCount = dosenCount;
            ViewBag.PicCount = picCount;
            ViewBag.CarGasolineCount = carGasolineCount;
            ViewBag.CarElectricCount = carElectricCount;
            return View();
        }
        private int GetDosenCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM PIC WHERE Status = 1", connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }
        private int GetPICCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Dosen WHERE Status = 1", connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }
        private int GetCarGasolineCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Data_Mobil WHERE Status = 1 AND jenis_kendaraan = 'Asset' AND fuel = 'Gasoline'", connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }
        private int GetCarElectricCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Data_Mobil WHERE Status = 1 AND jenis_kendaraan = 'Asset' AND fuel = 'Electric'", connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
