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
        
        public ItemModel getname(string? item_pemeriksaan)
        {
            ItemModel itmModel = new ItemModel();
            try
            {
                string query = "select count(*) from dbo.Itm_Pemeriksaan where item_pemeriksaan = @p1 AND status != 0";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", item_pemeriksaan);
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
        public void insertdata(ItemModel itmModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertItem";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@item_pemeriksaan", itmModel.item_pemeriksaan);
                command.Parameters.AddWithValue("@kategori_pemeriksaan", itmModel.kategori_pemeriksaan);
                command.Parameters.AddWithValue("@standart_pemeriksaan", itmModel.standart_pemeriksaan);
                command.Parameters.AddWithValue("@metode_pemeriksaan", itmModel.metode_pemeriksaan);
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
        public void updatedata(ItemModel itmModel)
        {
            try
            {
                string storedProcedureName = "sp_UpdateItem";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id_item", itmModel.id_item);
                command.Parameters.AddWithValue("@item_pemeriksaan", itmModel.item_pemeriksaan);
                command.Parameters.AddWithValue("@kategori_pemeriksaan", itmModel.kategori_pemeriksaan);
                command.Parameters.AddWithValue("@standart_pemeriksaan", itmModel.standart_pemeriksaan);
                command.Parameters.AddWithValue("@metode_pemeriksaan", itmModel.metode_pemeriksaan);
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
