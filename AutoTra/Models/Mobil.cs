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
                string query = "SELECT * FROM Data_Mobil where status != 0";
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

        public List<MobilModel> getSearch(string search)
        {
            List<MobilModel> mbllist = new List<MobilModel>();
            try
            {
                string query = "SELECT * FROM Data_Mobil where nama = @p1 AND status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", search);
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
            return mbllist;
        }

        public void insertdata(MobilModel mobilModel, bool allowNullVin = false)
        {
            try
            {
                string storedProcedureName = "sp_InsertMobil";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@jenis_kendaraan", mobilModel.jenis_mobil);
                command.Parameters.AddWithValue("@nama", mobilModel.nama);
                if (mobilModel.jenis_mobil == "Asset" && !allowNullVin)
                {
                    command.Parameters.AddWithValue("@vin", string.Empty); // Set VIN ke string kosong
                }
                else
                {
                    command.Parameters.AddWithValue("@vin", mobilModel.vin);
                }
                command.Parameters.AddWithValue("@no_engine", mobilModel.no_engine);
                command.Parameters.AddWithValue("@warna", mobilModel.warna);
                command.Parameters.AddWithValue("@kilometer", mobilModel.kilometer);
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

        public MobilModel getdata(int? id_mobil)
        {
            MobilModel mblmodel = new MobilModel();
            try
            {
                string query = "select * from dbo.Data_Mobil where id_mobil = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id_mobil);
                Console.WriteLine(id_mobil);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                mblmodel.id_mobil = Convert.ToInt32(reader["id_mobil"].ToString());
                mblmodel.jenis_mobil = reader["jenis_kendaraan"].ToString();
                mblmodel.nama = reader["nama"].ToString();
                mblmodel.vin = reader["vin"].ToString();
                mblmodel.no_engine = reader["no_engine"].ToString();
                mblmodel.warna = reader["warna"].ToString();
                mblmodel.kilometer = reader["kilometer"].ToString();
                mblmodel.bahan_bakar = reader["fuel"].ToString();
                mblmodel.status = Convert.ToInt32(reader["status"].ToString());
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

                command.Parameters.AddWithValue("@id_mobil", mobilModel.id_mobil);
                command.Parameters.AddWithValue("@jenis_kendaraan", mobilModel.jenis_mobil);
                Console.WriteLine(mobilModel.jenis_mobil);
                command.Parameters.AddWithValue("@nama", mobilModel.nama);
                Console.WriteLine(mobilModel.nama);
                command.Parameters.AddWithValue("@vin", mobilModel.vin);
                Console.WriteLine(mobilModel.vin);
                command.Parameters.AddWithValue("@no_engine", mobilModel.no_engine);
                Console.WriteLine(mobilModel.no_engine);
                command.Parameters.AddWithValue("@warna", mobilModel.warna);
                Console.WriteLine(mobilModel.warna);
                command.Parameters.AddWithValue("@kilometer", mobilModel.kilometer);
                Console.WriteLine(mobilModel.kilometer);
                command.Parameters.AddWithValue("@fuel", mobilModel.bahan_bakar);
                Console.WriteLine(mobilModel.bahan_bakar);
                command.Parameters.AddWithValue("@status", mobilModel.status);
                Console.WriteLine(mobilModel.status);

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
                Console.WriteLine(id_mobil);
                string storedProcedureName = "[dbo].[sp_DeleteMobil]";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_mobil", Convert.ToInt32(id_mobil));
                _connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting data Mobil: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

    }
}