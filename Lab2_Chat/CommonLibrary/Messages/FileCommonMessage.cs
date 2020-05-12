using System;
using System.Collections.Generic;
using System.Net;

namespace CommonLibrary
{
    [Serializable]
    public class FileCommonMessage : CommonChatMessage
    {
        public List<string> FileNames { get; }

        public FileCommonMessage(DateTime dateTime, IPAddress senderIp, int senderPort, string content, int senderId, List<string> fileNames) : base(dateTime, senderIp, senderPort, content, senderId)
        {
            FileNames = fileNames;
        }
    }
}
