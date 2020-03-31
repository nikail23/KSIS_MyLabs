using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public class ParticipantsListMessage : Message
    {
        public List<ChatParticipant> participants;

        public ParticipantsListMessage(DateTime dateTime, IPAddress senderIp, int senderPort, List<ChatParticipant> participants) : base(dateTime, senderIp, senderPort)
        {
            this.participants = participants;
        }
    }
}
