using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProject
{
    public class ServerInfo
    {
        public string ServerIp { get; }

        public int ServerPort { get; }

        public ServerInfo(string serverIp, int serverPort)
        {
            ServerIp = serverIp;
            ServerPort = serverPort;
        }
    }
}
