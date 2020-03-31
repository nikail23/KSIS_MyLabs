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
        public int ReceiverId { get; }

        public IndividualChatMessage(DateTime dateTime, IPAddress senderIp, int senderPort, string content, int senderId, int receiverId) : base(dateTime, senderIp, senderPort, content, senderId)
        {
            ReceiverId = receiverId;
        }
    }
}
