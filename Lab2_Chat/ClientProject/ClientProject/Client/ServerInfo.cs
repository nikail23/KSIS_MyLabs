using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientProject
{
    public class ServerInfo
    {
        public IPAddress ServerIp { get; }

        public int ServerPort { get; }

        public ServerInfo(IPAddress serverIp, int serverPort)
        {
            ServerIp = serverIp;
            ServerPort = serverPort;
        }
    }
}
