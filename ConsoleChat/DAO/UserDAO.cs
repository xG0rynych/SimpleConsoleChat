using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChat
{
    public class UserDAO
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

        public async Task<bool> AddUser(User user)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO Users([Login], [Password]) VALUES(@Login, @Password);", connection);
                    SqlParameter loginParam = new SqlParameter("@Login", user.Login);
                    SqlParameter passParam = new SqlParameter("@Password", user.Password);
                    command.Parameters.Add(loginParam);
                    command.Parameters.Add(passParam);
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public async void DeleteUser(string login)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("DELETE FROM Users WHERE [Login]=@Login;", connection);
                    SqlParameter loginParam = new SqlParameter("@Login", login);
                    command.Parameters.Add(loginParam);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<User> GetUserByLogin(string login)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT*FROM Users WHERE [Login]=@login;", connection);
                    command.Parameters.Add(new SqlParameter("@login", login));
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if(reader.HasRows)
                        {
                            await reader.ReadAsync();
                            return new User(reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT*FROM Users;", connection);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if(reader.HasRows)
                        {
                            List<User> users = new List<User>();
                            while (await reader.ReadAsync())
                            {
                                users.Add(new User(reader.GetString(0), reader.GetString(1)));
                            }
                            return users;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<User> UserAuthorization(string login, string password)
        {
            User user = new User(login, password);
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT*FROM Users WHERE [Login]=@login AND [Password]=@pass;", connection);
                    command.Parameters.Add(new SqlParameter("@login", user.Login));
                    command.Parameters.Add(new SqlParameter("@pass", user.Password));
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        return new User(reader.GetString(0), reader.GetString(1));
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        

    }
}
