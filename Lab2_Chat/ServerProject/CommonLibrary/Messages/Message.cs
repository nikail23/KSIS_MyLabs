using System;

namespace CommonLibrary.Messages
{
    public abstract class Message
    {
        public DateTime dateTime { get; }
        public string senderIp { get; }
        public int senderPort { get; }
        public Message(DateTime dateTime, string senderIp, int senderPort)
        {
            this.dateTime = dateTime;
            this.senderIp = senderIp;
            this.senderPort = senderPort;
        }
    }
}
