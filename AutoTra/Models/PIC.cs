﻿using System.Data;
using System.Data.SqlClient;

namespace AutoTra.Models
{
    public class PIC
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public PIC(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        public bool IsUsernameExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM PIC WHERE username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public List<PICModel> getAllData()
        {
            List<PICModel> filmList = new List<PICModel>();
            try
            {
                string query = "SELECT * FROM PIC";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PICModel adm = new  PICModel
                    {
                        nim = reader["nim"].ToString(),
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

        public List<PICModel> getSearch(string search)
        {
            int check = 0;
            List<PICModel> filmList = new List<PICModel>();
            try
            {
                string query = "SELECT * FROM PIC where nim = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", search);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PICModel adm = new PICModel
                    {
                        nim = reader["nim"].ToString(),
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
                check = 0;
            }
            if (check == 1)
            {
                try
                {
                    string query = "SELECT * FROM PIC where nama = @p1";
                    SqlCommand command = new SqlCommand(query, _connection);
                    command.Parameters.AddWithValue("@p1", search);
                    _connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        PICModel adm = new PICModel
                        {
                            nim = reader["nim"].ToString(),
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
            }
            return filmList;
        }

        public void insertData(PICModel picModel)
        {
            try
            {
                string storedProcedureName = "sp_InsertPIC";
                SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nim", picModel.nim);
                command.Parameters.AddWithValue("@nama", picModel.nama);
                command.Parameters.AddWithValue("@username", picModel.username);
                command.Parameters.AddWithValue("@password", picModel.password);
                command.Parameters.AddWithValue("@peran", picModel.peran);
                command.Parameters.AddWithValue("@status", picModel.status);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public PICModel getData(string nim)
        {
            PICModel picModel = new PICModel();
            try
            {
                string query = "select * from dbo.PIC where nim = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", nim);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                picModel.nim = reader["nim"].ToString();
                picModel.nama = reader["nama"].ToString();
                picModel.username = reader["username"].ToString();
                picModel.password = reader["password"].ToString();
                picModel.peran = reader["peran"].ToString();
                picModel.status = Convert.ToInt32(reader["status"].ToString());
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return picModel;
        }

        public void updateData(PICModel picModel)
        {
            try
            {
                string storedProcedureName = "sp_UpdatePIC";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nim", picModel.nim);
                command.Parameters.AddWithValue("@nama", picModel.nama);
                command.Parameters.AddWithValue("@username", picModel.username);
                command.Parameters.AddWithValue("@password", picModel.password);
                command.Parameters.AddWithValue("@peran", picModel.peran);
                command.Parameters.AddWithValue("@status", picModel.status);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void deleteData(string nim)
        {
            try
            {
                string storedProcedureName = "[dbo].[sp_DeletePIC]";
                using SqlCommand command = new SqlCommand(storedProcedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@nim", nim);
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