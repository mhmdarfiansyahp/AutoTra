using System.Data;
using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class DetailForm
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public DetailForm(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        public List<DetailFormModel> getAllData(int? id)
        {
            List<DetailFormModel> dtllist = new List<DetailFormModel>();
            try
            {
                string query = "SELECT * FROM dbo.Dtl_CRUD_Frm_Pemeriksaan where id_form = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DetailFormModel dtl = new DetailFormModel
                    {
                        id_form = Convert.ToInt32(reader["id_form"].ToString()),
                        id_item = Convert.ToInt32(reader["id_item"].ToString()),
                    };
                    dtllist.Add(dtl);
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
            return dtllist;
        }

        public DetailFormModel getdata(int? id_form)
        {
            DetailFormModel dtlmodel = new DetailFormModel();
            DetailFormModel dtlmodel1 = new DetailFormModel();
            try
            {
                string query = "select * from dbo.CRUD_Frm_Pemeriksaan where id_form = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id_form);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                dtlmodel1.id_form = Convert.ToInt32(reader["id_form"].ToString());
                dtlmodel.id_mobil = Convert.ToInt32(reader["id_mobil"].ToString());
                reader.Close();
                string query1 = "select * from dbo.Data_Mobil where id_mobil = @p2";
                SqlCommand command1 = new SqlCommand(query1, _connection);
                command1.Parameters.AddWithValue("@p2", dtlmodel.id_mobil);
                SqlDataReader reader1 = command1.ExecuteReader();
                reader1.Read();
                dtlmodel1.jenis_mobil = reader1["jenis_kendaraan"].ToString();
                dtlmodel1.nama = reader1["nama"].ToString();
                dtlmodel1.vin = reader1["vin"].ToString();
                dtlmodel1.no_engine = reader1["no_engine"].ToString();
                dtlmodel1.warna = reader1["warna"].ToString();
                dtlmodel1.kilometer = reader1["kilometer"].ToString();
                dtlmodel1.bahan_bakar = reader1["fuel"].ToString();
                dtlmodel1.status = Convert.ToInt32(reader1["status"].ToString());
                reader1.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dtlmodel1;
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

        public void insertdata(DetailFormModel dtlModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertDetailCRUDForm";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_form", dtlModel.id_form);
                command.Parameters.AddWithValue("@id_item", dtlModel.id_item);

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
                string storedProcedureName = "[dbo].[sp_DeleteDetailCRUDForm]";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_item", id);
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
