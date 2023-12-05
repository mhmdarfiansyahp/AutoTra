using System.Data;
using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class Kategori
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public Kategori(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        public List<KategoriModel> getAllData()
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
        public void insertdata(KategoriModel ktgModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertKategori";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nama", ktgModel.nama);
                command.Parameters.AddWithValue("@status", ktgModel.status);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public KategoriModel getname(string? nama)
        {
            KategoriModel ktgModel = new KategoriModel();
            try
            {
                string query = "select * from dbo.Kt_Pemeriksaan where nama = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", nama);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                ktgModel.id = Convert.ToInt32(reader["id_kategori"].ToString());
                ktgModel.nama = reader["nama"].ToString();
                ktgModel.status = Convert.ToInt32(reader["status"].ToString());
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ktgModel;
        }
        public KategoriModel getdata(int? id_std)
        {
            KategoriModel ktgModel = new KategoriModel();
            try
            {
                string query = "select * from dbo.Kt_Pemeriksaan where id_kategori = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id_std);
                Console.WriteLine(id_std);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                ktgModel.id = Convert.ToInt32(reader["id_kategori"].ToString());
                ktgModel.nama = reader["nama"].ToString();
                ktgModel.status = Convert.ToInt32(reader["status"].ToString());
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ktgModel;
        }
        public void updatedata(KategoriModel stdModel)
        {
            try
            {
                string storedProcedureName = "sp_UpdateKategori";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_kategori", stdModel.id);
                command.Parameters.AddWithValue("@nama", stdModel.nama);

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
                string storedProcedureName = "[dbo].[sp_DeleteKategori]";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_kategori", id);
                _connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting data Kategori Pemeriksaan: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

    }
}
