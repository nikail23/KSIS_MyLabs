using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public class ServerUdpAnswerMessage : Message
    {
        public string serverName { get; }
        public ServerUdpAnswerMessage(DateTime dateTime, IPAddress serverIp, int serverPort, string serverName) : base(dateTime, serverIp, serverPort) 
        {
            this.serverName = serverName;
        }
    }
}
