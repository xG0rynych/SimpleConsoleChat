using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChat
{
    public class Ivan
    {
        public int A { get; set; }
        public int B { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ChatManager chatManager = new ChatManager();
            Console.WriteLine("ЗДАРОВА");

            chatManager.Start();
        }
    }
}