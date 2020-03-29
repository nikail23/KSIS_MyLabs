using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public class CommonChatMessage : Message
    {
        public string senderName { get; }

        public string content { get; }

        public CommonChatMessage(DateTime dateTime, IPAddress senderIp, int senderPort, string senderName, string content) : base(dateTime, senderIp, senderPort)
        {
            this.senderName = senderName;
            this.content = content;
        }
    }
}
