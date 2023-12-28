using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class Notifikasi
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public Notifikasi(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<NotifikasiModel> GetNotifikasiListFromPgnUnitPraktek()
        {
            List<NotifikasiModel> notifikasiList = new List<NotifikasiModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT id_pgn_unit, npk, deskripsi, tanggal_pengajuan, status FROM Pgn_Unit_Praktek";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NotifikasiModel notifikasi = new NotifikasiModel
                            {
                                // Sesuaikan dengan kolom-kolom yang ada pada tabel Pgn_Unit_Praktek
                                id_pengajuan = Convert.ToInt32(reader["id_pgn_unit"].ToString()),
                                npk = reader["npk"].ToString(),
                                deskripsi = reader["deskripsi"].ToString(),
                                tanggl_pengajuan = reader.GetDateTime(reader.GetOrdinal("tanggal_pengajuan")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? 0 : Convert.ToInt32(reader["status"]),
                            };

                            notifikasiList.Add(notifikasi);
                        }
                    }
                }
            }

            return notifikasiList;
        }


    }


}