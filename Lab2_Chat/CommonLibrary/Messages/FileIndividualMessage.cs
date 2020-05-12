using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public class FileIndividualMessage : IndividualChatMessage
    {
        public List<string> FileNames { get; }

        public FileIndividualMessage(DateTime dateTime, IPAddress senderIp, int senderPort, string content, int senderId, int receiverId, List<string> fileNames) : base(dateTime, senderIp, senderPort, content, senderId, receiverId)
        {
            FileNames = fileNames;
        }
    }
}
