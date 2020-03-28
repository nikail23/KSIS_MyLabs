using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Messages
{
    class ClientUdpBroadcastRequestMessage : Message
    {
        public ClientUdpBroadcastRequestMessage(DateTime dateTime, string senderIp, int senderPort) : base(dateTime, senderIp, senderPort) { }
    }
}
