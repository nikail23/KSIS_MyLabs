using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Message = CommonLibrary.Message;

namespace ClientProject
{
    public partial class mainForm : Form
    {
        private const int DefaultSelectedDialog = 0;
        private Client client;
        private int selectedDialog;

        public mainForm()
        {
            selectedDialog = DefaultSelectedDialog;
            InitializeComponent();
            client = new Client(new BinaryMessageSerializer());
            client.ReceiveMessageEvent += ShowReceivedMessage;
            client.UnreadMessageEvent += UnreadMessageHandler;
            client.ReadMessageEvent += ReadMessageHandler;
            client.SearchServers();
        }

        private void ReadMessageHandler(Message message)
        {
            Action action = delegate
            {
                if (message is IndividualChatMessage)
                {
                    IndividualChatMessage individualChatMessage = (IndividualChatMessage)message;
                    if (client.id == individualChatMessage.ReceiverId)
                    {
                        participantsListBox.Items[individualChatMessage.SenderId] = client.participants[individualChatMessage.SenderId].Name;
                    }
                }
                else if (message is CommonChatMessage)
                {
                    participantsListBox.Items[0] = "Common chat";
                }
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void UnreadMessageHandler(string unreadMessageString, Message message)
        {
            Action action = delegate
            {
                if (message is IndividualChatMessage)
                {
                    IndividualChatMessage individualChatMessage = (IndividualChatMessage)message;
                    if ((client.id == individualChatMessage.ReceiverId)&&(selectedDialog != individualChatMessage.SenderId))
                    {
                        participantsListBox.Items[individualChatMessage.SenderId] = unreadMessageString;
                    }
                }
                else if (message is CommonChatMessage)
                {
                    participantsListBox.Items[0] = unreadMessageString;
                }
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void AddServerInfoToServersListBox(ServerUdpAnswerMessage serverUdpAnswerMessage)
        {
            Action action = delegate
            {
                string serverInfo = serverUdpAnswerMessage.ServerName;
                serversListBox.Items.Add(serverInfo);
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void AddNewCommonChatMessage(CommonChatMessage commonChatMessage)
        {
            Action action = delegate
            {
                if (selectedDialog == 0)
                {
                    string chatContent = "[" + commonChatMessage.DateTime.ToString() + " " + commonChatMessage.SenderIp.ToString() + ":" + commonChatMessage.SenderPort + "]: \"" + client.participants[commonChatMessage.SenderId].Name + "\": " + commonChatMessage.Content + "\r\n";
                    chatTextBox.Text += chatContent;
                }                        
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void RefreshParticipantsListBox(ParticipantsListMessage participantsListMessage)
        {
            Action action = delegate
            {
                participantsListBox.Items.Clear();
                foreach (ChatParticipant participant in participantsListMessage.participants)
                {
                    participantsListBox.Items.Add(participant.Name);
                }
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }       
        }

        private void HandleChatMessage(CommonChatMessage commonChatMessage)
        {
            if (commonChatMessage is IndividualChatMessage)
            {
                IndividualChatMessage individualChatMessage = (IndividualChatMessage)commonChatMessage;
                if (client.id == individualChatMessage.ReceiverId)
                {
                    ReceiverHandleIndividualChatMessage(individualChatMessage);
                }
                else if (client.id == individualChatMessage.SenderId)
                {
                    SenderHandleIndividualChatMessage(individualChatMessage);
                }
            }
            else if (commonChatMessage is CommonChatMessage)
            {
                AddNewCommonChatMessage(commonChatMessage);
            }
        }

        private void RefreshChatTextBox(List<Message> messageHistory)
        {
            Action action = delegate
            {
                chatTextBox.Clear();
                if ((messageHistory != null)&&(messageHistory.Count != 0))
                    foreach (Message message in messageHistory)
                    {
                        ShowReceivedMessage(message);
                    }
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }  
        }

        private void SenderHandleIndividualChatMessage(IndividualChatMessage individualChatMessage)
        {
            Action action = delegate
            {
                if (individualChatMessage.ReceiverId == selectedDialog)
                {
                    string chatContent = "[" + individualChatMessage.DateTime.ToString() + " " + individualChatMessage.SenderIp.ToString() + ":" + individualChatMessage.SenderPort + "]: \"" + client.participants[individualChatMessage.SenderId].Name + "\": " + individualChatMessage.Content + "\r\n";
                    chatTextBox.Text += chatContent;
                }
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void ReceiverHandleIndividualChatMessage(IndividualChatMessage individualChatMessage)
        {
            Action action = delegate
            {
                if (individualChatMessage.SenderId == selectedDialog)
                {
                    string chatContent = "[" + individualChatMessage.DateTime.ToString() + " " + individualChatMessage.SenderIp.ToString() + ":" + individualChatMessage.SenderPort + "]: \"" + client.participants[individualChatMessage.SenderId].Name + "\": " + individualChatMessage.Content + "\r\n";
                    chatTextBox.Text += chatContent;
                }
            };
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }  
        }

        public void ShowReceivedMessage(Message message)
        {
            if (message is ServerUdpAnswerMessage)
            {
                AddServerInfoToServersListBox((ServerUdpAnswerMessage)message);
            }
            else if (message is ParticipantsListMessage)
            {
                RefreshParticipantsListBox((ParticipantsListMessage)message);
            }
            else if (message is CommonChatMessage)
            {
                CommonChatMessage commonChatMessage = (CommonChatMessage)message;
                HandleChatMessage(commonChatMessage);
            } 
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Close();
        }

        private bool ClientUsernameCheck(ref string clientUsername)
        {
            if (clientNameTextBox.Text != "")
            {
                clientUsername = clientNameTextBox.Text;
                return true;
            }             
            MessageBox.Show("Client username field is empty!");
            return false;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string clientUsername = "";
            if (ClientUsernameCheck(ref clientUsername))
            {
                int serverIndex = serversListBox.SelectedIndex;
                client.ConnectToServer(serverIndex, clientUsername);
            }
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            client.SendMessage(messageTextBox.Text, selectedDialog);
            messageTextBox.Clear();
        }

        private bool UnreadMessageCheck(ChatParticipant chatParticipant)
        {
            if (chatParticipant.GetUnreadMessagesCount() != 0)
                return true;
            return false;
        }

        private void ChangeDialog()
        {
            ChatParticipant chatParticipant = client.participants[selectedDialog];
            if (UnreadMessageCheck(chatParticipant))
            {
                chatParticipant.SetUnreadMessageCountZero();
            }
            currentChatLabel.Text = client.participants[selectedDialog].Name;
            List<Message> newMessages = client.participants[selectedDialog].MessageHistory;
            RefreshChatTextBox(newMessages);
        }

        private void participantsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((participantsListBox.SelectedIndex != selectedDialog)&&(participantsListBox.SelectedIndex >= 0))
            {
                selectedDialog = participantsListBox.SelectedIndex;
                ChangeDialog();
            }
        }
    }
}
