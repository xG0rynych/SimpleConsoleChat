using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChat
{
    public class User
    {
        private string _login;
        private string _password;
        private List<Chat> _chats;

        public string Login { get=>_login; set=>_login=value; }
        public string Password
        {
            get => _password;
            set
            {
                byte[] tmpSource = Encoding.UTF8.GetBytes(value);
                byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
                _password = ByteArrToStr(tmpHash);
            }
        }
        public List<Chat> Chats { get => _chats; set=>_chats=value; }

        public User(string login, string password)
        {
            _chats = new List<Chat>();
            _login = login;
            Password = password;
        }

        private string ByteArrToStr(byte[] arr)
        {
            string result = string.Empty;
            for (int i = 0; i < arr.Length-1; i++)
            {
                result += arr[i].ToString("X2");
            }
            return result;
        }

        public void AddChat(User user, string chatName)
        {
            _chats.Add(new Chat(this, user, chatName));
        }

        public void RemoveChat(Chat chat)
        {
            _chats.Remove(chat);
        }
    }
}
