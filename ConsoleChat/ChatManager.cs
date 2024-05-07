using ConsoleChat.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChat
{
    public class ChatManager
    {
        private readonly UserDAO _userDAO = new UserDAO();
        private readonly MessageDAO _messageDAO = new MessageDAO();
        private readonly ChatDAO _chatDAO = new ChatDAO();
        private const string SYMBOLS = "~~~~~~~~~~~~~~~~~~~~~~~~";
        public void Start()
        {
            Console.WriteLine($"1 - Registration\n2 - Authorization\n{SYMBOLS}");
            string choice = Console.ReadLine();
            if(choice.Equals("1"))
            {
                Console.WriteLine("Enter login: ");
                string login = Console.ReadLine();

                Console.WriteLine("Enter password: ");
                string pass = Console.ReadLine();

                User user = new User(login, pass);
                bool isUserAdded = _userDAO.AddUser(user).Result;
                if(isUserAdded==false)
                {
                    Console.WriteLine($"User registration error, perhaps the username is already occupied.\n{SYMBOLS}\n");
                    Start();
                }
                else
                {
                    AfterRegistrationOrAuthorization(user);
                }
            }
            if (choice.Equals("2"))
            {
                Console.WriteLine("Enter login: ");
                string login = Console.ReadLine();

                Console.WriteLine("Enter password: ");
                string pass = Console.ReadLine();

                User user = _userDAO.UserAuthorization(login, pass).Result;
                if(user==null)
                {
                    Console.WriteLine($"User authorization error, try again.\n{SYMBOLS}\n");
                    Start();
                }
                else
                {
                    AfterRegistrationOrAuthorization(user);
                }
            }
            if(choice.Equals("1")==false && choice.Equals("2")==false)
            {
                Console.WriteLine("Please write 1 or 2.");
                Start();
            }
        }

        public void AfterRegistrationOrAuthorization(User user)
        {
            string variants = $"1 - show chats\n2 - create chats\n3 - exit\n{SYMBOLS}";
            while (5==5)
            {
                Console.WriteLine(variants);
                string choice = Console.ReadLine();
                if(choice.Equals("1"))
                {
                    ShowChats(user);
                }
                if (choice.Equals("2"))
                {
                    Console.Write("Write user login to add to the chat: ");
                    string login = Console.ReadLine();
                    User userAdd = _userDAO.GetUserByLogin(login).Result;
                    if(userAdd==null)
                    {
                        Console.WriteLine($"User not exist with login: {login}");
                        continue;
                    }
                    else
                    {
                        Console.Write("Write chat name: ");
                        string chatName = Console.ReadLine();
                        CreateChat(user, userAdd, chatName);
                    }
                }
                if(choice.Equals("3"))
                {
                    break;
                }
            }
        }

        public void ShowChats(User user)
        {
            List<Chat> chats = _chatDAO.GetChatsByLogin(user.Login).Result;
            if(chats==null)
            {
                Console.WriteLine("You don't have chats.");
                return;
            }
            else
            {
                for (int i = 0; i < chats.Count; i++)
                {
                    Console.WriteLine($"ID: {chats[i].Id}\n{chats[i].ChatName}\nAdmin: {chats[i].UserAdmin.Login}; User: {chats[i]._User.Login}\n{SYMBOLS}");
                }
                user.Chats = chats;
                string variants = $"For open chat write id, for exit - 0: ";
                while (5 == 5)
                {
                    Console.WriteLine(variants);
                    string choice = Console.ReadLine();
                    int chatId;
                    if(int.TryParse(choice, out chatId))
                    {
                        foreach (var chat in user.Chats)
                        {
                            if(chat.Id==chatId)
                            {
                                ShowMessages(chat, user);
                            }
                        }
                    }
                    if(choice.Equals("0"))
                    {
                        break;
                    }
                    if(choice.Equals("0")==false && int.TryParse(choice, out chatId)==false)
                    {
                        Console.WriteLine("Incorrect string, try again.");
                    }
                }
            }
        }

        public void CreateChat(User userAdmin, User user, string chatName)
        {
            Chat chat = new Chat(userAdmin, user, chatName);
            _chatDAO.AddChat(chat);
            userAdmin.AddChat(user, chatName);
            user.AddChat(userAdmin, chatName);
        }

        public void WriteMessage(Chat chat, string content, User fromUser, User toUser)
        {
            Message message = new Message(fromUser, toUser, chat, content);
            _messageDAO.AddMessage(message);
            chat.AddMessage(message);
        }

        public void ShowMessages(Chat chat, User user)
        {
            List<Message> messages = _messageDAO.GetMessagesByChat(chat.Id).Result;
            if(messages == null)
            {
                Console.WriteLine("No messages");
            }
            if(messages!=null)
            {
                foreach (var message in messages)
                {
                    Console.WriteLine($"{message.FromUser.Login}\n{message.DepartureDate}\n{message.Content}\n{SYMBOLS}");
                }
            }
            Console.WriteLine("1 - exit");
            while (5 == 5)
            {
                Console.Write("Send-> ");
                string content = Console.ReadLine();
                if (content.Equals("1"))
                {
                    return;
                }
                if(user.Login.Equals(chat.UserAdmin.Login))
                {
                    WriteMessage(chat, content, user, chat._User);
                    Console.WriteLine($"{chat.Messages[chat.Messages.Count - 1].FromUser.Login}\n" +
                        $"{chat.Messages[chat.Messages.Count - 1].DepartureDate}\n" +
                        $"{chat.Messages[chat.Messages.Count - 1].Content}\n{SYMBOLS}");
                }
                if(user.Login.Equals(chat._User.Login))
                {
                    WriteMessage(chat, content, user, chat.UserAdmin);
                    Console.WriteLine($"{chat.Messages[chat.Messages.Count - 1].FromUser.Login}\n" +
                        $"{chat.Messages[chat.Messages.Count - 1].DepartureDate}\n" +
                        $"{chat.Messages[chat.Messages.Count - 1].Content}\n{SYMBOLS}");
                }
            }
        }
    }
}
