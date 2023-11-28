using System.Data;
using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class Mobil
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public Mobil(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public List<MobilModel> getAllData()
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

        public void insertdata(MobilModel mobilModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertMobil";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@jenis_kendaraan", mobilModel.jenis_mobil);
                command.Parameters.AddWithValue("@nama", mobilModel.nama);
                command.Parameters.AddWithValue("@vin", mobilModel.vin);
                command.Parameters.AddWithValue("@no_engine", mobilModel.no_engine);
                command.Parameters.AddWithValue("@warna", mobilModel.warna);
                command.Parameters.AddWithValue("@status", mobilModel.kilometer);
                command.Parameters.AddWithValue("@fuel", mobilModel.bahan_bakar);
                command.Parameters.AddWithValue("@status", mobilModel.status);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public MobilModel getdata(int id_mobil)
        {
            MobilModel mblmodel = new MobilModel();
            try
            {
                string query = "select * from dbo.Data_Mobil where id_mobil = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id_mobil);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                mblmodel.jenis_mobil = reader["jenis_mobil"].ToString();
                mblmodel.nama = reader["nama"].ToString();
                mblmodel.vin = reader["vin"].ToString();
                mblmodel.no_engine = reader["no_engine"].ToString();
                mblmodel.warna = reader["warna"].ToString();
                mblmodel.kilometer = reader["kilometer"].ToString();
                mblmodel.bahan_bakar = reader["fuel"].ToString();
                mblmodel.status = Convert.ToInt32(reader["fuel"].ToString());
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return mblmodel;
        }

        public void updatedata(MobilModel mobilModel)
        {
            try
            {
                string storedProcedureName = "sp_UpdateMobil";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@jenis_kendaraan", mobilModel.jenis_mobil);
                command.Parameters.AddWithValue("@nama", mobilModel.nama);
                command.Parameters.AddWithValue("@vin", mobilModel.vin);
                command.Parameters.AddWithValue("@no_engine", mobilModel.no_engine);
                command.Parameters.AddWithValue("@warna", mobilModel.warna);
                command.Parameters.AddWithValue("@status", mobilModel.kilometer);
                command.Parameters.AddWithValue("@fuel", mobilModel.bahan_bakar);
                command.Parameters.AddWithValue("@status", mobilModel.status);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void deletedata(string id_mobil)
        {
            try
            {
                using SqlCommand command = new SqlCommand("sp_DeleteMobil", _connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id_mobil", id_mobil);
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
