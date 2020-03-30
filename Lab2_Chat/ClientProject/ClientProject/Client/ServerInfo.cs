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
        public string ServerName { get; }

        public IPAddress ServerIp { get; }

        public int ServerPort { get; }

        public ServerInfo(string serverName, IPAddress serverIp, int serverPort)
        {
            ServerName = serverName;
            ServerIp = serverIp;
            ServerPort = serverPort;
        }
    }
}
