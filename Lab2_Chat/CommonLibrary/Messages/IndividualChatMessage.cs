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
        public string receiverName { get; }

        public IndividualChatMessage(DateTime dateTime, IPAddress senderIp, int senderPort, string senderName, string content, string receiverName) : base(dateTime, senderIp, senderPort, senderName, content)
        {
            this.receiverName = receiverName;
        }
    }
}
