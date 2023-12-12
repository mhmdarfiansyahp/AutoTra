using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace AutoTra.Models
{
    public class PengajuanKendaraan
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public PengajuanKendaraan(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public List<PengajuanKendaraanModel> getAllData()
        {
            List<PengajuanKendaraanModel> mbllist = new List<PengajuanKendaraanModel>();
            try
            {
                string query = "SELECT * FROM Pgn_Unit_Praktek";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PengajuanKendaraanModel pengajuan = new PengajuanKendaraanModel
                    {
                        id_pengajuan = Convert.ToInt32(reader["id_pgn_unit"].ToString()),
                        tanggl_pengajuan = reader.GetDateTime(reader.GetOrdinal("tanggal_pengajuan")),
                        nim = reader["nim"].ToString(),
                        npk = reader["npk"].ToString(),
                        id_mobil = Convert.ToInt32(reader["id_mobil"].ToString()),
                        deskripsi = reader["deskripsi"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    mbllist.Add(pengajuan);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }
            return mbllist;
        }

        public void insertdata(PengajuanKendaraanModel pengajuanmodel)
        {
            try
            {
                string storedProcedureName = "[sp_InsertPenggunaanUnit]";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@tanggal_pengajuan", pengajuanmodel.tanggl_pengajuan);
                command.Parameters.AddWithValue("@nim", pengajuanmodel.nim);
                command.Parameters.AddWithValue("@npk", pengajuanmodel.npk);
                command.Parameters.AddWithValue("@id_mobil", pengajuanmodel.id_mobil);
                command.Parameters.AddWithValue("@deskripsi", pengajuanmodel.deskripsi);
                command.Parameters.AddWithValue("@status", pengajuanmodel.status);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
