using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public delegate void UnreadMessageDelegate(string unreadMessageString, Message message);
    public delegate void ReadMessageDelegate(Message message);

    [Serializable]
    public class ChatParticipant
    {
        public string Name { get; }
        public int Id { get; }
        public List<Message> MessageHistory { get; set; }
        public string UnreadMessageString 
        {
            get
            {
                return "You have " + UnreadMessagesCount + " new message(s)!";
            }
        }
        private int UnreadMessagesCount;
        public event UnreadMessageDelegate UnreadMessageEvent;
        public event ReadMessageDelegate ReadMessageEvent;

        public ChatParticipant(string name, int id, List<Message> messageHistory)
        {
            Name = name;
            Id = id;
            MessageHistory = messageHistory;
            UnreadMessagesCount = 0;
        }

        public int GetUnreadMessagesCount()
        {
            return UnreadMessagesCount;
        }

        public void SetUnreadMessageCountZero()
        {
            UnreadMessagesCount = 0;
            ReadMessageEvent(MessageHistory[MessageHistory.Count - 1]);
        }

        public void UnreadMessagesCountIncrement(CommonChatMessage commonChatMessage)
        {
            UnreadMessagesCount++;
            UnreadMessageEvent(UnreadMessageString, commonChatMessage);
        }
    }
}
