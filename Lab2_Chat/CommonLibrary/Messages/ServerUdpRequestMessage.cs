using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    class ServerUdpRequestMessage : Message
    {
        public ServerUdpRequestMessage(DateTime dateTime, string senderIp, int senderPort) : base(dateTime, senderIp, senderPort) { }
    }
}
