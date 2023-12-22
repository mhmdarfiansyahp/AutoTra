using Microsoft.AspNetCore.Mvc;
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
        public void insertdetailpemeriksaan(List<PengajuanKendaraanModel> pengajuan)
        {
            foreach (var pengajuanmodel in pengajuan)
            {
                try
                {

                    if (pengajuanmodel.hasil_inspeksi == "Yes" || pengajuanmodel.alasan == null)
                    {
                        pengajuanmodel.alasan = "null";
                    }
                    else if (pengajuanmodel.hasil_inspeksi == null || pengajuanmodel.alasan != null)
                    {
                        pengajuanmodel.hasil_inspeksi = "null";
                    }else if (pengajuanmodel.hasil_inspeksi == null && pengajuanmodel.alasan == null)
                    {
                        return;
                    }
                    string storedProcedureName = "sp_InsertDetailPemeriksaan";
                    using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@id_pemeriksaan", pengajuanmodel.id_pemeriksaan);
                    command.Parameters.AddWithValue("@id_item", pengajuanmodel.id_item);
                    command.Parameters.AddWithValue("@hasil", pengajuanmodel.hasil_inspeksi);
                    command.Parameters.AddWithValue("@alasan", pengajuanmodel.alasan);
                    command.Parameters.AddWithValue("@id_pengajuan", pengajuanmodel.id_pengajuan);

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

        public void insertdetailpemeriksaanfinal(List<PengajuanKendaraanModel> pengajuan)
        {
            foreach (var pengajuanmodel in pengajuan)
            {
                try
                {

                    if (pengajuanmodel.hasil_inspeksi == "Yes" || pengajuanmodel.alasan == null)
                    {
                        pengajuanmodel.alasan = "null";
                    }
                    else if (pengajuanmodel.hasil_inspeksi == null || pengajuanmodel.alasan != null)
                    {
                        pengajuanmodel.hasil_inspeksi = "null";
                    }
                    else if (pengajuanmodel.hasil_inspeksi == null && pengajuanmodel.alasan == null)
                    {
                        return;
                    }
                    string storedProcedureName = "sp_InsertDetailPemeriksaan2";
                    using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@id_pemeriksaan", pengajuanmodel.id_pemeriksaan);
                    command.Parameters.AddWithValue("@id_item", pengajuanmodel.id_item);
                    command.Parameters.AddWithValue("@hasil", pengajuanmodel.hasil_inspeksi);
                    command.Parameters.AddWithValue("@alasan", pengajuanmodel.alasan);
                    command.Parameters.AddWithValue("@id_pengajuan", pengajuanmodel.id_pengajuan);

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

        public List<PengajuanKendaraanModel> getForm(int? id)
        {
            PengajuanKendaraanModel dtlmodel = new PengajuanKendaraanModel();
            PengajuanKendaraanModel dtlmodel1 = new PengajuanKendaraanModel(); 
            List<PengajuanKendaraanModel> formlist = new List<PengajuanKendaraanModel>();
            try
            {
                string query = "select * from dbo.Pgn_Unit_Praktek where id_pgn_unit = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dtlmodel.id_mobil = Convert.ToInt32(reader["id_mobil"].ToString());
                    dtlmodel.skala = reader["skala"].ToString();
                    dtlmodel.nim = reader["nim"].ToString();
                }
                reader.Close();

                // Query kedua
                string jenis_form = "First Check";
                string query1 = "SELECT * FROM dbo.CRUD_Frm_Pemeriksaan WHERE id_mobil = @p2 AND skala = @p3 AND jenis_form = @jenis_form AND status <> 0";
                SqlCommand command1 = new SqlCommand(query1, _connection);
                command1.Parameters.AddWithValue("@p2", dtlmodel.id_mobil);
                command1.Parameters.AddWithValue("@p3", dtlmodel.skala);
                command1.Parameters.AddWithValue("@jenis_form", jenis_form);
                SqlDataReader reader1 = command1.ExecuteReader();    
                    if (reader1.Read())
                    {
                        dtlmodel1.id_form = Convert.ToInt32(reader1["id_form"].ToString());
                    }
                    reader1.Close();

                // Query ketiga
                string query2 = "SELECT * FROM dbo.Dtl_CRUD_Frm_Pemeriksaan where id_form = @p6";
                SqlCommand command2 = new SqlCommand(query2, _connection);
                command2.Parameters.AddWithValue("@p6", dtlmodel1.id_form);
                SqlDataReader reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    PengajuanKendaraanModel form = new PengajuanKendaraanModel
                    {
                        id_form = Convert.ToInt32(reader2["id_form"].ToString()),
                        id_item = Convert.ToInt32(reader2["id_item"].ToString())
                    };
                    formlist.Add(form);
                }
                reader2.Close();
                _connection.Close();
            }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    return formlist;
        }

        public PengajuanKendaraanModel getPemeriksaan(int? id)
        {
            PengajuanKendaraanModel dtlmodel = new PengajuanKendaraanModel();
            try
            {
                string query = "select * from dbo.Pgn_Unit_Praktek where id_pgn_unit = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dtlmodel.id_mobil = Convert.ToInt32(reader["id_mobil"].ToString());
                    dtlmodel.skala = reader["skala"].ToString();
                    dtlmodel.nim = reader["nim"].ToString();
                }
                reader.Close();

                string query1 = "SELECT * FROM dbo.CRUD_Frm_Pemeriksaan WHERE id_mobil = @p2 AND skala = @p3 AND status <> 0";
                SqlCommand command1 = new SqlCommand(query1, _connection);
                command1.Parameters.AddWithValue("@p2", dtlmodel.id_mobil);
                command1.Parameters.AddWithValue("@p3", dtlmodel.skala);
                SqlDataReader reader1 = command1.ExecuteReader();
                if (reader1.Read())
                {
                    dtlmodel.id_form = Convert.ToInt32(reader1["id_form"].ToString());
                }
                reader1.Close();

                string query2 = "SELECT * FROM dbo.Pemeriksaan WHERE id_form = @p4 AND nim = @p5 AND [status] = 0";
                SqlCommand command2 = new SqlCommand(query2, _connection);
                command2.Parameters.AddWithValue("@p4", dtlmodel.id_form); // dtlmodel.id_form belum diisi sebelumnya
                command2.Parameters.AddWithValue("@p5", dtlmodel.nim); // dtlmodel.nim belum diisi sebelumnya
                SqlDataReader reader2 = command2.ExecuteReader();
                if (reader2.Read())
                {
                    dtlmodel.id_pemeriksaan = Convert.ToInt32(reader2["id_pemeriksaan"].ToString());
                }
                reader2.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dtlmodel;
        }

        public PengajuanKendaraanModel getPemeriksaan1(int? id)
        {
            PengajuanKendaraanModel dtlmodel = new PengajuanKendaraanModel();
            try
            {
                string query = "select * from dbo.Pgn_Unit_Praktek where id_pgn_unit = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dtlmodel.id_mobil = Convert.ToInt32(reader["id_mobil"].ToString());
                    dtlmodel.skala = reader["skala"].ToString();
                    dtlmodel.nim = reader["nim"].ToString();
                }
                reader.Close();

                string query1 = "SELECT * FROM dbo.CRUD_Frm_Pemeriksaan WHERE id_mobil = @p2 AND skala = @p3 AND status <> 0";
                SqlCommand command1 = new SqlCommand(query1, _connection);
                command1.Parameters.AddWithValue("@p2", dtlmodel.id_mobil);
                command1.Parameters.AddWithValue("@p3", dtlmodel.skala);
                SqlDataReader reader1 = command1.ExecuteReader();
                if (reader1.Read())
                {
                    dtlmodel.id_form = Convert.ToInt32(reader1["id_form"].ToString());
                }
                reader1.Close();

                string query2 = "SELECT * FROM dbo.Pemeriksaan WHERE id_form = @p4 AND nim = @p5 AND [status] = 2";
                SqlCommand command2 = new SqlCommand(query2, _connection);
                command2.Parameters.AddWithValue("@p4", dtlmodel.id_form); // dtlmodel.id_form belum diisi sebelumnya
                command2.Parameters.AddWithValue("@p5", dtlmodel.nim); // dtlmodel.nim belum diisi sebelumnya
                SqlDataReader reader2 = command2.ExecuteReader();
                if (reader2.Read())
                {
                    dtlmodel.id_pemeriksaan = Convert.ToInt32(reader2["id_pemeriksaan"].ToString());
                }
                reader2.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dtlmodel;
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
