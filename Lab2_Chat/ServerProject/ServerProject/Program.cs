using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProject
{
    class Program
    {
        private const string ServerIp = "127.0.0.1";
        private const int ServerPort = 5555;
        static void Main(string[] args)
        {
            Server server = new Server(new BinaryMessageSerializer());
            server.Start(ServerIp, ServerPort);
        }
    }
}
