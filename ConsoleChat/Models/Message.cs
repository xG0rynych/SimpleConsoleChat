using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChat
{
    public class Message
    {
        private int _id;
        private DateTime _departureTime;
        private User _fromUser;
        private User _toUser;
        private Chat _chat;
        private string _content;

        public int Id { get=>_id; set=>_id=value; }
        public DateTime DepartureDate { get => _departureTime; set => _departureTime = value; }
        public User FromUser { get=>_fromUser; }
        public User ToUser { get=>_toUser; }
        public Chat _Chat { get=>_chat; }
        public string Content { get=>_content; set=>_content=value; }

        public Message(User fromUser, User toUser, Chat chat, string content)
        {
            _departureTime = DateTime.UtcNow;
            _fromUser = fromUser;
            _toUser = toUser;
            _chat = chat;
            _content = content;
        }
    }
}
