using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public class ClientUdpRequestMessage : Message
    {
        public ClientUdpRequestMessage(DateTime dateTime, IPAddress senderIp, int senderPort) : base(dateTime, senderIp, senderPort) { }
    }
}
