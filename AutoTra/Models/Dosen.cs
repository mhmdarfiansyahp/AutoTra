﻿using System.Data;
using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class Dosen
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public Dosen(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        public bool IsUsernameExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Dosen WHERE username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public List<DosenModel> getAllData()
        {
            List<DosenModel> dsnlist = new List<DosenModel>();
            try
            {
                string query = "SELECT * FROM Dosen";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DosenModel adm = new DosenModel
                    {
                        npk = reader["npk"].ToString(),
                        nama = reader["nama"].ToString(),
                        username = reader["username"].ToString(),
                        password = reader["password"].ToString(),
                        peran = reader["peran"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    dsnlist.Add(adm);
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
            return dsnlist;
        }

        public List<DosenModel> getSearch(string cari)
        {
            int check = 0;
            List<DosenModel> dsnlist = new List<DosenModel>();
            try
            {
                string query = "SELECT * FROM Dosen where nama = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", cari);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DosenModel adm = new DosenModel
                    {
                        npk = reader["npk"].ToString(),
                        nama = reader["nama"].ToString(),
                        username = reader["username"].ToString(),
                        password = reader["password"].ToString(),
                        peran = reader["peran"].ToString(),
                        status = Convert.ToInt32(reader["status"].ToString()),
                    };
                    check = 1;
                    dsnlist.Add(adm);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                check = 0;
            }
            if (check == 0)
            {
                try
                {
                    string query = "SELECT * FROM Dosen where npk = @p1";
                    SqlCommand command = new SqlCommand(query, _connection);
                    command.Parameters.AddWithValue("@p1", cari);
                    _connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DosenModel adm = new DosenModel
                        {
                            npk = reader["npk"].ToString(),
                            nama = reader["nama"].ToString(),
                            username = reader["username"].ToString(),
                            password = reader["password"].ToString(),
                            peran = reader["peran"].ToString(),
                            status = Convert.ToInt32(reader["status"].ToString()),
                        };
                        check = 1;
                        dsnlist.Add(adm);
                    }
                    reader.Close();
                    _connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dsnlist;
        }


        public void insertdata(DosenModel dosenModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertDosen";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@npk", dosenModel.npk);
                command.Parameters.AddWithValue("@nama", dosenModel.nama);
                command.Parameters.AddWithValue("@username", dosenModel.username);
                command.Parameters.AddWithValue("@password", dosenModel.password);
                command.Parameters.AddWithValue("@peran", dosenModel.peran);
                command.Parameters.AddWithValue("@status", dosenModel.status);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DosenModel getdata(int npk)
        {
            DosenModel dsnmodel = new DosenModel();
            try
            {
                string query = "select * from Dosen where npk = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", npk);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                dsnmodel.npk = reader["npk"].ToString();
                dsnmodel.nama = reader["nama"].ToString();
                dsnmodel.username = reader["username"].ToString();
                dsnmodel.password = reader["password"].ToString();
                dsnmodel.peran = reader["peran"].ToString();
                dsnmodel.status = Convert.ToInt32(reader["status"].ToString());
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dsnmodel;
        }

        public void updatedata(DosenModel dsnModel)
        {
            try
            {
                string storedProcedureName = "sp_UpdateDosen";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@npk", dsnModel.npk);
                command.Parameters.AddWithValue("@nama", dsnModel.nama);
                command.Parameters.AddWithValue("@username", dsnModel.username);
                command.Parameters.AddWithValue("@password", dsnModel.password);
                command.Parameters.AddWithValue("@peran", dsnModel.peran);
                command.Parameters.AddWithValue("@status", dsnModel.status);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void deletedata(string npk)
        {
            try
            {
                string storedProcedureName = "[dbo].[sp_DeleteDosen]";
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
