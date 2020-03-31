using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public class IndividualChatMessage : CommonChatMessage
    {
        public string ReceiverName { get; }

        public IndividualChatMessage(DateTime dateTime, IPAddress senderIp, int senderPort, string content, string senderName, string receiverName) : base(dateTime, senderIp, senderPort, content, senderName)
        {
            ReceiverName = receiverName;
        }
    }
}
