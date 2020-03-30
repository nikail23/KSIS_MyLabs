using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerProject
{
    class ClientInfo
    {
        public string name { get; }

        public Socket socket { get; }

        public ClientInfo(string name, Socket socket)
        {
            this.name = name;
            this.socket = socket;
        }
    }
}
