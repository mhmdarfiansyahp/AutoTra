using System.Data;
using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class Form
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public Form(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        public List<FormModel> getAllData()
        {
            List<FormModel> formlist = new List<FormModel>();
            try
            {
                string query = "SELECT * FROM dbo.CRUD_Frm_Pemeriksaan where status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FormModel form = new FormModel
                    {
                        id_form = Convert.ToInt32(reader["id_form"].ToString()),
                        id_mobil = Convert.ToInt32(reader["id_mobil"].ToString()),
                        skala = reader["skala"].ToString(),
                        jenis_form = reader["jenis_form"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    formlist.Add(form);
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
            return formlist;
        }

        public List<FormModel> getSearchForm(string search)
        {
            List<FormModel> formlist = new List<FormModel>();
            FormModel formmodel = new FormModel();
            try
            {
                string query = "SELECT * FROM dbo.Data_Mobil where nama LIKE @p1 AND status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", "%" + search + "%");
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    formmodel.id_mobil = Convert.ToInt32(reader["id_mobil"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _connection.Close();

            try
            {
                string query = "SELECT * FROM dbo.CRUD_Frm_Pemeriksaan where id_mobil = @p2 AND status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p2", formmodel.id_mobil);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FormModel form = new FormModel
                    {
                        id_form = Convert.ToInt32(reader["id_form"].ToString()),
                        id_mobil = Convert.ToInt32(reader["id_mobil"].ToString()),
                        skala = reader["skala"].ToString(),
                        jenis_form = reader["jenis_form"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    formlist.Add(form);
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
            return formlist;
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
        public List<MobilModel> getAllCar()
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
        public void insertdata(FormModel formModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertCRUDForm";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_mobil", formModel.id_mobil);
                command.Parameters.AddWithValue("@skala", formModel.skala);
                command.Parameters.AddWithValue("@jenis_form", formModel.jenis_form);
                command.Parameters.AddWithValue("@status", formModel.status);


                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public ItemModel getdata(int? id)
        {
            ItemModel itmModel = new ItemModel();
            try
            {
                string query = "select * from dbo.Itm_Pemeriksaan where id_item = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                itmModel.id_item = Convert.ToInt32(reader["id_item"].ToString());
                itmModel.item_pemeriksaan = reader["item_pemeriksaan"].ToString();
                itmModel.kategori_pemeriksaan = reader["kategori_pemeriksaan"].ToString();
                itmModel.standart_pemeriksaan = reader["standart_pemeriksaan"].ToString();
                itmModel.metode_pemeriksaan = reader["metode_pemeriksaan"].ToString();
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
        public void deletedata(int id)
        {
            try
            {
                string storedProcedureName = "[dbo].[sp_DeleteCRUDForm]";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_form", id);
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
