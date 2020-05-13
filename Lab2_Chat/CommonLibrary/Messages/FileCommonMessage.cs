using System;
using System.Collections.Generic;
using System.Net;

namespace CommonLibrary
{
    [Serializable]
    public class FileCommonMessage : CommonChatMessage
    {
        public Dictionary<int, string> Files { get; }

        public FileCommonMessage(DateTime dateTime, IPAddress senderIp, int senderPort, string content, int senderId, Dictionary<int, string> files) : base(dateTime, senderIp, senderPort, content, senderId)
        {
            Files = files;
        }
    }
}
