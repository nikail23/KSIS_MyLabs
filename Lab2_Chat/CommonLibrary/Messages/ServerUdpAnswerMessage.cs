﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class ServerUdpAnswerMessage : Message
    {
        public ServerUdpAnswerMessage(DateTime dateTime, string senderIp, int senderPort) : base(dateTime, senderIp, senderPort) { }
    }
}
