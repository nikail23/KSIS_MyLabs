﻿using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
<<<<<<< HEAD
    public delegate void UnreadMessageDelegate(string unreadMessageString, Message message);
    public delegate void ReadMessage(Message message);

=======
>>>>>>> b61bdcec28773017d25d42a0f891ddc287a7600e
    [Serializable]
    public class ChatParticipant
    {
        public string Name { get; }
<<<<<<< HEAD
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
        public event ReadMessage ReadMessageEvent;
=======

        public int Id { get; }

        public List<Message> MessageHistory { get; set; }
>>>>>>> b61bdcec28773017d25d42a0f891ddc287a7600e

        public ChatParticipant(string name, int id, List<Message> messageHistory)
        {
            Name = name;
            Id = id;
            MessageHistory = messageHistory;
<<<<<<< HEAD
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

        public void UnreadMessagesCountIncrement()
        {
            UnreadMessagesCount++;
            UnreadMessageEvent(UnreadMessageString, MessageHistory[MessageHistory.Count - 1]);
=======
>>>>>>> b61bdcec28773017d25d42a0f891ddc287a7600e
        }
    }
}
