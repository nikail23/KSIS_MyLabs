using System;
using System.Net;

namespace CommonLibrary
{
    [Serializable]
    public abstract class Message
    {
        public DateTime DateTime { get; }

        public IPAddress SenderIp { get; }

        public int SenderPort { get; }

        public Message(DateTime dateTime, IPAddress senderIp, int senderPort)
        {
            DateTime = dateTime;
            SenderIp = senderIp;
            SenderPort = senderPort;
        }
    }
}
