using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public class ChatParticipant
    {
        public string Name { get; }

        public int Id { get; }

        public List<Message> MessageHistory { get; set; }

        public ChatParticipant(string name, int id, List<Message> messageHistory)
        {
            Name = name;
            Id = id;
            MessageHistory = messageHistory;
        }
    }
}
