using System.Data;
using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class Item
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public Item(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        public List<ItemModel> getAllData()
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
                        nama = reader["nama"].ToString(),
                        id_standart = Convert.ToInt32(reader["id_standart"].ToString()),
                        id_kategori = Convert.ToInt32(reader["id_kategori"].ToString()),
                        metode_inspeksi = reader["metode_inspeksi"].ToString(),
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
        public List<StandarModel> getAllStd()
        {
            List<StandarModel> stdlist = new List<StandarModel>();
            try
            {
                string query = "SELECT * FROM dbo.Std_Pemeriksaan where status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    StandarModel std = new StandarModel
                    {
                        id = Convert.ToInt32(reader["id_standart"].ToString()),
                        nama = reader["nama"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    stdlist.Add(std);
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
            return stdlist;
        }
        public List<StandarModel> getAllStdIndex()
        {
            List<StandarModel> stdlist = new List<StandarModel>();
            try
            {
                string query = "SELECT * FROM dbo.Std_Pemeriksaan";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    StandarModel std = new StandarModel
                    {
                        id = Convert.ToInt32(reader["id_standart"].ToString()),
                        nama = reader["nama"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    stdlist.Add(std);
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
            return stdlist;
        }
        public List<KategoriModel> getAllKtg()
        {
            List<KategoriModel> ktglist = new List<KategoriModel>();
            try
            {
                string query = "SELECT * FROM dbo.Kt_Pemeriksaan where status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    KategoriModel ktg = new KategoriModel
                    {
                        id = Convert.ToInt32(reader["id_kategori"].ToString()),
                        nama = reader["nama"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    ktglist.Add(ktg);
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
            return ktglist;
        }
        public List<KategoriModel> getAllKtgIndex()
        {
            List<KategoriModel> ktglist = new List<KategoriModel>();
            try
            {
                string query = "SELECT * FROM dbo.Kt_Pemeriksaan";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    KategoriModel ktg = new KategoriModel
                    {
                        id = Convert.ToInt32(reader["id_kategori"].ToString()),
                        nama = reader["nama"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    ktglist.Add(ktg);
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
            return ktglist;
        }
        public ItemModel getname(string? nama)
        {
            ItemModel itmModel = new ItemModel();
            try
            {
                string query = "select count(*) from dbo.Itm_Pemeriksaan where nama = @p1 AND status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", nama);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                itmModel.id_item = Convert.ToInt32(reader["id_item"].ToString());
                itmModel.nama = reader["nama"].ToString();
                itmModel.id_standart = Convert.ToInt32(reader["id_standart"].ToString());
                itmModel.id_kategori = Convert.ToInt32(reader["id_kategori"].ToString());
                itmModel.metode_inspeksi = reader["metode_inspeksi"].ToString();
                itmModel.status = Convert.ToInt32(reader["status"].ToString());
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return itmModel;
        }
        public void insertdata(ItemModel itmModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertItem";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nama", itmModel.nama);
                command.Parameters.AddWithValue("@id_standart", itmModel.id_standart);
                command.Parameters.AddWithValue("@id_kategori", itmModel.id_kategori);
                command.Parameters.AddWithValue("@metode_inspeksi", itmModel.metode_inspeksi);
                command.Parameters.AddWithValue("@status", itmModel.status);


                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public ItemModel getdata(int? id_itm)
        {
            ItemModel itmModel = new ItemModel();
            try
            {
                string query = "select * from dbo.Itm_Pemeriksaan where id_item = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id_itm);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                itmModel.id_item = Convert.ToInt32(reader["id_item"].ToString());
                itmModel.nama = reader["nama"].ToString();
                itmModel.id_standart = Convert.ToInt32(reader["id_standart"].ToString());
                itmModel.id_kategori = Convert.ToInt32(reader["id_kategori"].ToString());
                itmModel.metode_inspeksi = reader["metode_inspeksi"].ToString();
                itmModel.status = Convert.ToInt32(reader["status"].ToString());
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return itmModel;
        }
        public void updatedata(ItemModel itmModel)
        {
            try
            {
                string storedProcedureName = "sp_UpdateItem";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_item", itmModel.id_item);
                command.Parameters.AddWithValue("@nama", itmModel.nama);
                command.Parameters.AddWithValue("@id_standart", itmModel.id_standart);
                command.Parameters.AddWithValue("@id_kategori", itmModel.id_kategori);
                command.Parameters.AddWithValue("@metode_inspeksi", itmModel.metode_inspeksi);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void deletedata(int? id)
        {
            try
            {
                string storedProcedureName = "[dbo].[sp_DeleteItem]";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_item", id);
                _connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting data Item Pemeriksaan: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

    }
}
