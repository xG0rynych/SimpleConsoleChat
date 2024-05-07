using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChat.DAO
{
    public class MessageDAO
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
        private readonly UserDAO _userDAO = new UserDAO();
        private readonly ChatDAO _chatDAO = new ChatDAO();

        public async void AddMessage(Message message)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "INSERT INTO Messages VALUES(@depDate, @fromUser, @toUser, @idChat, @content);";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add(new SqlParameter("@depDate", message.DepartureDate));
                    command.Parameters.Add(new SqlParameter("@fromUser", message.FromUser.Login));
                    command.Parameters.Add(new SqlParameter("@toUser", message.ToUser.Login));
                    command.Parameters.Add(new SqlParameter("@idChat", message._Chat.Id));
                    command.Parameters.Add(new SqlParameter("@content", message.Content));
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public async Task<List<Message>> GetMessagesByChat(int chatId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM Messages WHERE IdChat=@idChat;", connection);
                    command.Parameters.Add(new SqlParameter("@idChat", chatId));
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if(reader.HasRows)
                        {
                            List<Message> messages = new List<Message>();
                            while(await reader.ReadAsync())
                            {
                                User fromUser = _userDAO.GetUserByLogin(reader.GetString(2)).Result;
                                User toUser = _userDAO.GetUserByLogin(reader.GetString(3)).Result;
                                string content = reader.GetString(5);
                                Chat chat = _chatDAO.GetChatById(reader.GetInt32(4)).Result;
                                Message message = new Message(fromUser, toUser, chat, content);
                                message.Id = reader.GetInt32(0);
                                message.DepartureDate = reader.GetDateTime(1);
                                messages.Add(message);
                            }
                            return messages;
                        }
                    }
                }
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
