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
                                skala = reader["skala"].ToString(),
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
        public DaftarPengajuanModel getdetailacc1(int? id_pengajuan)
        {
            DaftarPengajuanModel dtlacc = new DaftarPengajuanModel();
            DaftarPengajuanModel dtlacc1 = new DaftarPengajuanModel();
            try
            {
                string query = "select * from dbo.Pgn_Unit_Praktek where id_pgn_unit = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id_pengajuan);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                dtlacc1.id_pengajuan = Convert.ToInt32(reader["id_pgn_unit"].ToString());
                dtlacc1.id_mobil = Convert.ToInt32(reader["id_mobil"].ToString());
                dtlacc1.nim = reader["nim"].ToString();
                dtlacc1.skala = reader["skala"].ToString();
                dtlacc1.deskripsi = reader["deskripsi"].ToString();
                reader.Close();

                string query1 = "select * from dbo.Data_Mobil where id_mobil = @p2";
                SqlCommand command1 = new SqlCommand(query1, _connection);
                command1.Parameters.AddWithValue("@p2", dtlacc1.id_mobil);
                SqlDataReader reader1 = command1.ExecuteReader();
                reader1.Read();
                dtlacc1.jenis_mobil = reader1["jenis_kendaraan"].ToString();
                dtlacc1.nama_mobil = reader1["nama"].ToString();
                dtlacc1.vin = reader1["vin"].ToString();
                dtlacc1.no_engine = reader1["no_engine"].ToString();
                dtlacc1.warna = reader1["warna"].ToString();
                dtlacc1.kilometer = reader1["kilometer"].ToString();
                dtlacc1.bahan_bakar = reader1["fuel"].ToString();
                reader1.Close();

                string query2 = "select * from dbo.PIC where nim = @p3";
                SqlCommand command2 = new SqlCommand(query2, _connection);
                command2.Parameters.AddWithValue("@p3", dtlacc1.nim);
                SqlDataReader reader2 = command2.ExecuteReader();
                reader2.Read();
                dtlacc1.nim = reader2["nim"].ToString();
                dtlacc1.nama = reader2["nama"].ToString();
                reader2.Close();

                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dtlacc1;
        }
        public void approval1(int approvalStatus, int id_pengajuan, int id_mobil, DateTime tanggal_pemeriksaan, string Skala, string NIM, int status_pemeriksaan)
        {
            DaftarPengajuanModel dtlacc = new DaftarPengajuanModel();
            try
            {
                string query1 = "select * from dbo.CRUD_Frm_Pemeriksaan where skala = @p1 AND id_mobil = @p2";
                SqlCommand command1 = new SqlCommand(query1, _connection);
                command1.Parameters.AddWithValue("@p1", Skala);
                command1.Parameters.AddWithValue("@p2", id_mobil);
                _connection.Open();
                SqlDataReader reader = command1.ExecuteReader();
                reader.Read();
                dtlacc.id_form = Convert.ToInt32(reader["id_form"].ToString());
                reader.Close();
                _connection.Close();

                string storedProcedureName = "sp_ApprovalPengajuan1";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_pgn_unit", id_pengajuan);
                command.Parameters.AddWithValue("@id_mobil", id_mobil);
                command.Parameters.AddWithValue("@status", approvalStatus);
                command.Parameters.AddWithValue("@tanggal_pemeriksaan", tanggal_pemeriksaan);
                command.Parameters.AddWithValue("@id_form", dtlacc.id_form);
                command.Parameters.AddWithValue("@nim", NIM);
                command.Parameters.AddWithValue("@status_pemeriksaan", status_pemeriksaan);

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
