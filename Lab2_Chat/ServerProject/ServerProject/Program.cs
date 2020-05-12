using CommonLibrary;
using FileSharingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProject
{
    class Program
    {
        private const string FileSharingServerUrl = "http://localhost:8888/";

        static void Main(string[] args)
        {
            Server server = new Server(new BinaryMessageSerializer());
            FileSharingServer fileSharingServer = new FileSharingServer(FileSharingServerUrl);
            server.Start();
            fileSharingServer.Start();
        }
    }
}
