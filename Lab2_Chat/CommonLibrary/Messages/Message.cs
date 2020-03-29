using System;
using System.Net;

namespace CommonLibrary
{
    [Serializable]
    public abstract class Message
    {
        public DateTime dateTime { get; }

        public IPAddress senderIp { get; }

        public int senderPort { get; }

        public Message(DateTime dateTime, IPAddress senderIp, int senderPort)
        {
            this.dateTime = dateTime;
            this.senderIp = senderIp;
            this.senderPort = senderPort;
        }
    }
}
