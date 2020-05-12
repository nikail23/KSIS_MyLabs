using CommonLibrary;
using FileSharingLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Message = CommonLibrary.Message;

namespace ClientProject
{
    public partial class mainForm : Form
    {
        private const string FileSharingServerUrl = "http://localhost:8888/";
        private const int DefaultSelectedDialog = 0;
        private const string CommonChatName = "Common chat";
        private const int CommonChatId = 0;

        private Client client;
        private FileSharingClient fileSharingClient;
        private int selectedDialog;

        public mainForm()
        {
            selectedDialog = DefaultSelectedDialog;
            InitializeComponent();
            client = new Client(new BinaryMessageSerializer());
            fileSharingClient = new FileSharingClient();
            client.ReceiveMessageEvent += ShowReceivedMessage;
            client.UnreadMessageEvent += UnreadMessageHandler;
            client.ReadMessageEvent += ReadMessageHandler;
            fileSharingClient.UpdateFilesListEvent += UpdateFileList;
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
                        ParticipantsListBox.Items[individualChatMessage.SenderId] = client.participants[individualChatMessage.SenderId].Name;
                    }
                }
                else if (message is CommonChatMessage)
                {
                    ParticipantsListBox.Items[CommonChatId] = CommonChatName;
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

        private void UpdateFileList(Dictionary<int, string> files)
        {
            FilesInfoListBox.Items.Clear();
            foreach (KeyValuePair<int, string> file in files)
            {
                FilesInfoListBox.Items.Add(file.Value);
            }
        }

        private void UnreadMessageHandler(string unreadMessageString, Message message)
        {
            Action action = delegate
            {
                if (message is IndividualChatMessage)
                {
                    IndividualChatMessage individualChatMessage = (IndividualChatMessage)message;
                    if ((client.id == individualChatMessage.ReceiverId) && (selectedDialog != individualChatMessage.SenderId))
                    {
                        ParticipantsListBox.Items[individualChatMessage.SenderId] = unreadMessageString;
                    }
                }
                else if ((message is CommonChatMessage) && (selectedDialog != CommonChatId))
                {
                    ParticipantsListBox.Items[CommonChatId] = unreadMessageString;
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
                ServersListBox.Items.Add(serverInfo);
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
                    ChatTextBox.Text += chatContent;
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
                ParticipantsListBox.Items.Clear();
                foreach (ChatParticipant participant in participantsListMessage.participants)
                {
                    ParticipantsListBox.Items.Add(participant.Name);
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
                ChatTextBox.Clear();
                if ((messageHistory != null) && (messageHistory.Count != 0))
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
                    ChatTextBox.Text += chatContent;
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
                    ChatTextBox.Text += chatContent;
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
            if (ClientNameTextBox.Text != "")
            {
                clientUsername = ClientNameTextBox.Text;
                return true;
            }
            MessageBox.Show("Client username field is empty!");
            return false;
        }

        private void EnableFormComponents()
        {
            SendMessageButton.Enabled = true;
            AddFileButton.Enabled = true;
            DeleteFileButton.Enabled = true;
            ShowFilesButton.Enabled = true;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string clientUsername = "";
            if (ClientUsernameCheck(ref clientUsername))
            {
                int serverIndex = ServersListBox.SelectedIndex;
                if (client.ConnectToServer(serverIndex, clientUsername))
                {
                    EnableFormComponents();
                }
            }
        }

        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            if (FilesInfoListBox.Items.Count == 0)
            {
                client.SendMessage(MessageTextBox.Text, selectedDialog);
            }
            else
            {
                client.SendMessage(MessageTextBox.Text, selectedDialog);
            }
            MessageTextBox.Clear();
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

        private void ParticipantsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ParticipantsListBox.SelectedIndex != selectedDialog) && (ParticipantsListBox.SelectedIndex >= 0))
            {
                selectedDialog = ParticipantsListBox.SelectedIndex;
                ChangeDialog();
            }
        }

        private async void AddFileButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = openFileDialog.FileName;
                try
                {
                    await fileSharingClient.SendFile(filePath, FileSharingServerUrl);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Исключение: " + ex.Message);
                }
            }
        }

        private async void ShowFilesButton_Click(object sender, EventArgs e)
        {
            try
            {
                fileSharingClient.UpdateFilesListEvent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Исключение: " + ex.Message);
            }
        }
    }
}
