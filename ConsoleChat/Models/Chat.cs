using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChat
{
    public class Chat
    {
        private int _id;
        private string _chatName;
        private User _userAdmin;
        private User _user;
        private List<Message> _messages;

        public int Id { get => _id; set => _id = value; }
        public string ChatName { get=>_chatName; set=>_chatName=value; }
        public User UserAdmin { get=>_userAdmin; set=>_user=value; }
        public User _User { get=>_user; set=>_user=value; }
        public List<Message> Messages { get=>_messages; }

        public Chat(User userAdmin, User user, string chatName)
        {
            _messages = new List<Message>();
            _userAdmin = userAdmin;
            _user = user;
            _chatName = chatName;
        }

        public void AddMessage(Message message)
        {
            _messages.Add(message);
        }

        public void RemoveMessage(Message message)
        {
            _messages.Remove(message);
        }
    }
}
