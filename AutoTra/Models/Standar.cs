using System.Data;
using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class Standar
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public Standar(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        public List<StandarModel> getAllData()
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
        public void insertdata(StandarModel stdModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertStandart";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nama", stdModel.nama);
                command.Parameters.AddWithValue("@status", stdModel.status);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public StandarModel getname(string? nama)
        {   
            StandarModel stdModel = new StandarModel();
            try
            {
                string query = "select * from dbo.Std_Pemeriksaan where nama = @p1 AND status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", nama);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                stdModel.id = Convert.ToInt32(reader["id_standart"].ToString());
                stdModel.nama = reader["nama"].ToString();
                stdModel.status = Convert.ToInt32(reader["status"].ToString());
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return stdModel;
        }
        public StandarModel getdata(int? id_std)
        {
            StandarModel stdModel = new StandarModel();
            try
            {
                string query = "select * from dbo.Std_Pemeriksaan where id_standart = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id_std);
                Console.WriteLine(id_std);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                stdModel.id = Convert.ToInt32(reader["id_standart"].ToString());
                stdModel.nama = reader["nama"].ToString();
                stdModel.status = Convert.ToInt32(reader["status"].ToString());
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return stdModel;
        }
        public void updatedata(StandarModel stdModel)
        {
            try
            {
                string storedProcedureName = "sp_UpdateStandart";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_standart", stdModel.id);
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

        public void deletedata(int id)
        {
            try
            {
                string storedProcedureName = "[dbo].[sp_DeleteStandart]";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_standart", id);
                _connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting data Standar Pemeriksaan: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

    }
}
