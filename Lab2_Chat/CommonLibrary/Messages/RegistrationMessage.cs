using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public class RegistrationMessage : Message
    {
        public string ClientName { get; }

        public RegistrationMessage(DateTime dateTime, IPAddress clientIp, int clientPort, string clientName) : base(dateTime, clientIp, clientPort)
        {
            ClientName = clientName;
        }
    }
}
