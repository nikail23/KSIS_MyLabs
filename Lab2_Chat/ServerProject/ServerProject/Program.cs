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
        static void Main(string[] args)
        {
            Server server = new Server(new BinaryMessageSerializer());
            server.Start();
            //while (true)
            //{
            //    string serverCommand = Console.ReadLine();
            //    if (serverCommand == "stop")
            //        server.Close();
            //}
        }
    }
}
