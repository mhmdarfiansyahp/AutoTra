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

        public List<PengajuanKendaraanModel> getPengajuan(int? id)
        {
            List<PengajuanKendaraanModel> mbllist = new List<PengajuanKendaraanModel>();
            try
            {
                string query = "SELECT * FROM Pgn_Unit_Praktek where id_pgn_unit = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
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
                    }
                    else if (pengajuanmodel.hasil_inspeksi == null && pengajuanmodel.alasan == null)
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

                    string formattedDate = pengajuanmodel.tanggl_pengajuan.ToString("yyyy-MM-dd");
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
                    dtlmodel.tanggl_pengajuan = Convert.ToDateTime(reader["tanggal_pengajuan"]);

                }
                reader.Close();

                string jenis_form = "Final Check";
                string query1 = "SELECT * FROM dbo.CRUD_Frm_Pemeriksaan WHERE id_mobil = @p2 AND skala = @p3 AND jenis_form = @p6 AND status = 1";
                SqlCommand command1 = new SqlCommand(query1, _connection);
                command1.Parameters.AddWithValue("@p2", dtlmodel.id_mobil);
                command1.Parameters.AddWithValue("@p3", dtlmodel.skala);
                command1.Parameters.AddWithValue("@p6", jenis_form);
                SqlDataReader reader1 = command1.ExecuteReader();
                if (reader1.Read())
                {
                    dtlmodel.id_form = Convert.ToInt32(reader1["id_form"].ToString());
                }
                reader1.Close();

                string formattedDate = dtlmodel.tanggl_pengajuan.ToString("yyyy-MM-dd");
                string query2 = "SELECT * FROM dbo.Pemeriksaan WHERE id_form = @p4 AND nim = @p5 AND [status] = 3 AND tanggal_pemeriksaan = @p7";
                SqlCommand command2 = new SqlCommand(query2, _connection);
                command2.Parameters.AddWithValue("@p4", dtlmodel.id_form); // dtlmodel.id_form belum diisi sebelumnya
                command2.Parameters.AddWithValue("@p5", dtlmodel.nim); // dtlmodel.nim belum diisi sebelumnya
                command2.Parameters.AddWithValue("@p7", formattedDate);
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

        public List<MobilModel> getAllCarData()
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
        public List<PengajuanKendaraanModel> getdetailpemeriksaan()
        {
            List<PengajuanKendaraanModel> dtlacc = new List<PengajuanKendaraanModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string queryUnits = "SELECT * FROM dbo.Pgn_Unit_Praktek WHERE status = 5";

                    using (SqlCommand commandUnits = new SqlCommand(queryUnits, connection))
                    {
                        using (SqlDataReader readerUnits = commandUnits.ExecuteReader())
                        {
                            while (readerUnits.Read())
                            {
                                PengajuanKendaraanModel pmodel = new PengajuanKendaraanModel
                                {
                                    id_pengajuan = Convert.ToInt32(readerUnits["id_pgn_unit"]),
                                    tanggl_pengajuan = Convert.ToDateTime(readerUnits["tanggal_pengajuan"]),
                                    id_mobil = Convert.ToInt32(readerUnits["id_mobil"]),
                                    npk = readerUnits["npk"].ToString(),
                                    nim = readerUnits["nim"].ToString(),
                                    skala = readerUnits["skala"].ToString(),
                                    deskripsi = readerUnits["deskripsi"].ToString()
                                };

                                dtlacc.Add(pmodel); // Add to dtlacc1, not dtlacc
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Consider logging the exception for further analysis
            }

            return dtlacc;
        }
        public List<PICModel> getAllPIC()
        {
            List<PICModel> filmList = new List<PICModel>();
            try
            {
                string query = "SELECT * FROM PIC";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PICModel adm = new PICModel
                    {
                        nim = reader["nim"].ToString(),
                        nama = reader["nama"].ToString(),
                        username = reader["username"].ToString(),
                        password = reader["password"].ToString(),
                        peran = reader["peran"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    filmList.Add(adm);
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
            return filmList;
        }
        public List<PengajuanKendaraanModel> getDetailPemeriksaan(int? id)
        {
            List<PengajuanKendaraanModel> dtllist = new List<PengajuanKendaraanModel>();
            try
            {
                string query = "SELECT * FROM dbo.Pgn_Unit_Praktek WHERE id_pgn_unit = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PengajuanKendaraanModel dtllist2 = new PengajuanKendaraanModel
                    {
                        skala = reader["skala"].ToString(),
                        nim = reader["nim"].ToString(),
                        tanggl_pengajuan = Convert.ToDateTime(reader["tanggal_pengajuan"])
                    };

                    string formattedDate = dtllist2.tanggl_pengajuan.ToString("yyyy-MM-dd");
                    reader.Close(); // Close the first reader before opening a new one

                    string queryPemeriksaan = "SELECT id_pemeriksaan FROM dbo.Pemeriksaan WHERE nim = @p3 AND tanggal_pemeriksaan = @p5 AND [status] = 4";
                    SqlCommand commandPemeriksaan = new SqlCommand(queryPemeriksaan, _connection);
                    commandPemeriksaan.Parameters.AddWithValue("@p3", dtllist2.nim);
                    commandPemeriksaan.Parameters.AddWithValue("@p5", formattedDate);

                    SqlDataReader readerPemeriksaan = commandPemeriksaan.ExecuteReader();

                    if (readerPemeriksaan.Read())
                    {
                        int idPemeriksaan = Convert.ToInt32(readerPemeriksaan["id_pemeriksaan"]);
                        readerPemeriksaan.Close(); // Close the second reader before opening a new one

                        string queryDetailPemeriksaan = "SELECT * FROM dbo.Detail_Pemeriksaan WHERE id_pemeriksaan = @p4";
                        SqlCommand commandDetailPemeriksaan = new SqlCommand(queryDetailPemeriksaan, _connection);
                        commandDetailPemeriksaan.Parameters.AddWithValue("@p4", idPemeriksaan);

                        SqlDataReader readerDetailPemeriksaan = commandDetailPemeriksaan.ExecuteReader();

                        while (readerDetailPemeriksaan.Read())
                        {
                            PengajuanKendaraanModel pmodel = new PengajuanKendaraanModel
                            {
                                id_item = Convert.ToInt32(readerDetailPemeriksaan["id_item"]),
                                hasil_inspeksi = readerDetailPemeriksaan["hasil_inspeksi"].ToString(),
                                alasan = readerDetailPemeriksaan["alasan_tidak_sesuai"].ToString()
                            };
                            dtllist.Add(pmodel);
                        }
                        readerDetailPemeriksaan.Close();
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
            return dtllist;
        }

        public List<PengajuanKendaraanModel> getdetailpemeriksaan2(int id)
        {
            List<PengajuanKendaraanModel> dtlacc = new List<PengajuanKendaraanModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string queryUnits = "SELECT * FROM dbo.Pgn_Unit_Praktek WHERE id_pgn_unit = @p1 AND status = 5";

                    using (SqlCommand commandUnits = new SqlCommand(queryUnits, connection))
                    {
                        commandUnits.Parameters.AddWithValue("@p1", id);
                        using (SqlDataReader readerUnits = commandUnits.ExecuteReader())
                        {
                            while (readerUnits.Read())
                            {
                                PengajuanKendaraanModel pmodel = new PengajuanKendaraanModel
                                {
                                    id_pengajuan = Convert.ToInt32(readerUnits["id_pgn_unit"]),
                                    tanggl_pengajuan = Convert.ToDateTime(readerUnits["tanggal_pengajuan"]),
                                    id_mobil = Convert.ToInt32(readerUnits["id_mobil"]),
                                    npk = readerUnits["npk"].ToString(),
                                    nim = readerUnits["nim"].ToString(),
                                    skala = readerUnits["skala"].ToString(),
                                    deskripsi = readerUnits["deskripsi"].ToString()
                                };

                                dtlacc.Add(pmodel); // Add to dtlacc1, not dtlacc
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Consider logging the exception for further analysis
            }

            return dtlacc;
        }

        public List<PengajuanKendaraanModel> getSearchLaporan(string search)
        {
            List<PengajuanKendaraanModel> dtlacc = new List<PengajuanKendaraanModel>(); // List untuk menyimpan hasil query pertama
            List<int> idMobils = new List<int>(); // List untuk menyimpan nilai id_mobil

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT DISTINCT id_mobil FROM Data_Mobil WHERE nama LIKE @p1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@p1", "%" + search + "%");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int idMobil = Convert.ToInt32(reader["id_mobil"]);
                                idMobils.Add(idMobil); // Menambahkan id_mobil ke dalam list
                            }
                        }
                    }

                    // Gunakan nilai idMobils langsung dalam klausa IN untuk query kedua
                    string queryUnits = "SELECT * FROM dbo.Pgn_Unit_Praktek WHERE id_mobil = @p2 AND status = 5";

                    using (SqlCommand commandUnits = new SqlCommand(queryUnits, connection))
                    {
                        foreach (int idMobil in idMobils)
                        {
                            commandUnits.Parameters.Clear();
                            commandUnits.Parameters.AddWithValue("@p2", idMobil);

                            using (SqlDataReader readerUnits = commandUnits.ExecuteReader())
                            {
                                while (readerUnits.Read())
                                {
                                    PengajuanKendaraanModel pmodel = new PengajuanKendaraanModel
                                    {
                                        id_pengajuan = Convert.ToInt32(readerUnits["id_pgn_unit"]),
                                        tanggl_pengajuan = Convert.ToDateTime(readerUnits["tanggal_pengajuan"]),
                                        id_mobil = Convert.ToInt32(readerUnits["id_mobil"]),
                                        npk = readerUnits["npk"].ToString(),
                                        nim = readerUnits["nim"].ToString(),
                                        skala = readerUnits["skala"].ToString(),
                                        deskripsi = readerUnits["deskripsi"].ToString()
                                    };

                                    dtlacc.Add(pmodel); // Tambahkan hasil ke dalam list
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Pertimbangkan untuk mencatat pesan kesalahan untuk analisis lebih lanjut
            }

            return dtlacc;
        }



    }
}
