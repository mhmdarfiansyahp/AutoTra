using System.Data;
using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class Admin
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public Admin(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public List<AdminModel> getAllData()
        {
            List<AdminModel> filmList = new List<AdminModel>();
            try
            {
                string query = "SELECT * FROM Admin";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AdminModel adm = new AdminModel
                    {
                        npk = reader["npk"].ToString(),
                        nama = reader["nama"].ToString(),
                        username = reader["username"].ToString(),
                        password = reader["password"].ToString(),
                        peran = reader["peran"].ToString(),
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

        public void insertData(AdminModel adminModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertAdmin";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@npk", adminModel.npk);
                command.Parameters.AddWithValue("@nama", adminModel.nama);
                command.Parameters.AddWithValue("@username", adminModel.username);
                command.Parameters.AddWithValue("@password", adminModel.password);
                command.Parameters.AddWithValue("@peran", adminModel.peran);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public AdminModel getData(int npk)
        {
            AdminModel admModel = new AdminModel();
            try
            {
                string query = "select * from dbo.Admin where npk = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", npk);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                admModel.npk = reader["npk"].ToString();
                admModel.nama = reader["nama"].ToString();
                admModel.username = reader["username"].ToString();
                admModel.password = reader["password"].ToString();
                admModel.peran = reader["peran"].ToString();
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return admModel;
        }

        public void updateData(AdminModel admModel)
        {
            try
            {
                string storedProcedureName = "sp_UpdateAdmin";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@npk", admModel.npk);
                command.Parameters.AddWithValue("@nama", admModel.nama);
                command.Parameters.AddWithValue("@username", admModel.username);
                command.Parameters.AddWithValue("@password", admModel.password);
                command.Parameters.AddWithValue("@peran", admModel.peran);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void deleteData(string npk)
        {
            try
            {
                using SqlCommand command = new SqlCommand("sp_DeleteAdmin", _connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@npk", npk);
                _connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting data: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

    }
}
