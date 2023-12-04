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
            List<StandarModel> mbllist = new List<StandarModel>();
            try
            {
                string query = "SELECT * FROM Std_Pemeriksaan where status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    StandarModel mbl = new StandarModel
                    {
                        id = Convert.ToInt32(reader["id"].ToString()),
                        nama = reader["nama"].ToString(),
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
        public void insertdata(StandarModel stdModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertStandart";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id", stdModel.id);
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
        public StandarModel getdata(int id_std)
        {
            StandarModel stdModel = new StandarModel();
            try
            {
                string query = "select * from dbo.Std_Pemeriksaan where id = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id_std);
                Console.WriteLine(id_std);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                stdModel.id = Convert.ToInt32(reader["id"].ToString());
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
    }
}
