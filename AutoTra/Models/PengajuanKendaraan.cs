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
                        skala = reader["skala"].ToString(),
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
                command.Parameters.AddWithValue("@skala", pengajuanmodel.skala);
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

        public List<PengajuanKendaraanModel> getForm(int? id)
        {
            PengajuanKendaraanModel dtlmodel = new PengajuanKendaraanModel();
            PengajuanKendaraanModel dtlmodel1 = new PengajuanKendaraanModel();
            try
            {
                string query = "SELECT * FROM dbo.Pgn_Unit_Praktek where id_pgn_unit = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                dtlmodel.id_pengajuan = Convert.ToInt32(reader["id_pgn_unit"].ToString());
                dtlmodel.id_mobil = Convert.ToInt32(reader["id_mobil"].ToString());
                dtlmodel.skala = reader["skala"].ToString();
                reader.Close();

                string query1 = "select * from dbo.CRUD_Frm_Pemeriksaan where id_mobil = @p2 AND skala = @p3";
                SqlCommand command1 = new SqlCommand(query1, _connection);
                command1.Parameters.AddWithValue("@p2", dtlmodel.id_mobil);
                command1.Parameters.AddWithValue("@p3", dtlmodel.skala);
                SqlDataReader reader1 = command1.ExecuteReader();
                reader1.Read();
                dtlmodel1.id_form = Convert.ToInt32(reader1["id_form"].ToString());
                dtlmodel1.jenis_form = reader1["jenis_form"].ToString();
                reader1.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            List<PengajuanKendaraanModel> formlist = new List<PengajuanKendaraanModel>();
            try
            {
                string query = "SELECT * FROM dbo.Dtl_CRUD_Frm_Pemeriksaan where id_form = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", dtlmodel1.id_form);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PengajuanKendaraanModel form = new PengajuanKendaraanModel
                    {
                        id_form = Convert.ToInt32(reader["id_form"].ToString()),
                        id_item = Convert.ToInt32(reader["id_item"].ToString()),
                    };
                    formlist.Add(form);
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
            return formlist;
        }

        public List<MobilModel> getAllCarIndex()
        {
            List<MobilModel> mbllist = new List<MobilModel>();
            try
            {
                string query = "SELECT * FROM Data_Mobil";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MobilModel mbl = new MobilModel
                    {
                        id_mobil = Convert.ToInt32(reader["id_mobil"].ToString()),
                        jenis_mobil = reader["jenis_kendaraan"].ToString(),
                        nama = reader["nama"].ToString(),
                        vin = reader["vin"].ToString(),
                        no_engine = reader["no_engine"].ToString(),
                        warna = reader["warna"].ToString(),
                        kilometer = reader["kilometer"].ToString(),
                        bahan_bakar = reader["fuel"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    mbllist.Add(mbl);
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
        public List<ItemModel> getDataItem()
        {
            List<ItemModel> itmlist = new List<ItemModel>();
            try
            {
                string query = "SELECT * FROM dbo.Itm_Pemeriksaan where status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ItemModel itm = new ItemModel
                    {
                        id_item = Convert.ToInt32(reader["id_item"].ToString()),
                        item_pemeriksaan = reader["item_pemeriksaan"].ToString(),
                        kategori_pemeriksaan = reader["kategori_pemeriksaan"].ToString(),
                        standart_pemeriksaan = reader["standart_pemeriksaan"].ToString(),
                        metode_pemeriksaan = reader["metode_pemeriksaan"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    itmlist.Add(itm);
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
            return itmlist;
        }
    }
}
