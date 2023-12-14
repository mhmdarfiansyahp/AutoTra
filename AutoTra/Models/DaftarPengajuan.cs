using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace AutoTra.Models
{
    public class DaftarPengajuan
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public DaftarPengajuan(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public List<DaftarPengajuanModel> getAllData(string npk)
        {
            List<DaftarPengajuanModel> mbllist = new List<DaftarPengajuanModel>();

            try
            {
                // Use npk in the query to filter data
                string query = $"SELECT * FROM Pgn_Unit_Praktek WHERE npk = '{npk}'";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    _connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DaftarPengajuanModel pengajuan = new DaftarPengajuanModel
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
                    }
                }
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

    }
}
