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
        public string Content { get; }

        public string SenderName { get; }

        public CommonChatMessage(DateTime dateTime, IPAddress senderIp, int senderPort, string content, string senderName) : base(dateTime, senderIp, senderPort)
        {
            Content = content;
            SenderName = senderName;
        }
    }
}
