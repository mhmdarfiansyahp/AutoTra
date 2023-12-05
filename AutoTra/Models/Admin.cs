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

        public bool IsUsernameExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Admin WHERE username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public AdminModel getDataByUsername_Password(string username, string password)
        {
            AdminModel admModel = new AdminModel();
            try
            {
                string query = "SELECT * FROM Admin WHERE username = @username AND password = @password";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // User found
                    admModel.npk = reader["npk"].ToString();
                    admModel.nama = reader["nama"].ToString();
                    admModel.username = reader["username"].ToString();
                    admModel.password = reader["password"].ToString();
                    admModel.peran = reader["peran"].ToString();
                    admModel.status = Convert.ToInt32(reader["status"].ToString());
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
            return admModel.status != 0 ? admModel : null;
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

        public void insertData(AdminModel adminModel)
        {
            try
            {
                string storedProcedureName = "[sp_InsertAdmin]";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@npk", adminModel.npk);
                command.Parameters.AddWithValue("@nama", adminModel.nama);
                command.Parameters.AddWithValue("@username", adminModel.username);
                command.Parameters.AddWithValue("@password", adminModel.password);
                command.Parameters.AddWithValue("@peran", adminModel.peran);
                command.Parameters.AddWithValue("@status", adminModel.status);

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
                string query = "select * from Admin where npk = @p1";
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
                admModel.status = Convert.ToInt32(reader["status"].ToString());
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
                command.Parameters.AddWithValue("@status", admModel.status);

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
                string storedProcedureName = "[dbo].[sp_DeleteAdmin]";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
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
