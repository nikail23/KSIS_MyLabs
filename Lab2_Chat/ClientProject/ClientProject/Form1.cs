using CommonLibrary;
using FileSharingLibrary;
using System;
using System.Collections.Generic;
using System.IO;
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
            fileSharingClient.UpdateFilesToLoadListEvent += UpdateFileList;
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
            FilesTempListBox.Items.Clear();
            foreach (var file in files)
            {
                FilesTempListBox.Items.Add(file.Value);
            }
        }

        private void UnreadMessageHandler(string unreadMessageString, Message message)
        {
            Action action = delegate
            {
                if (message is IndividualChatMessage)
                {
                    var individualChatMessage = (IndividualChatMessage)message;
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
                var serverInfo = serverUdpAnswerMessage.ServerName;
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
                    var chatContent = "[" + commonChatMessage.DateTime.ToString() + " " + commonChatMessage.SenderIp.ToString() + ":" + commonChatMessage.SenderPort + "]: \"" + client.participants[commonChatMessage.SenderId].Name + "\": " + commonChatMessage.Content + "\r\n";
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
                foreach (var participant in participantsListMessage.participants)
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
                var individualChatMessage = (IndividualChatMessage)commonChatMessage;
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
                    foreach (var message in messageHistory)
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
                    var chatContent = "[" + individualChatMessage.DateTime.ToString() + " " + individualChatMessage.SenderIp.ToString() + ":" + individualChatMessage.SenderPort + "]: \"" + client.participants[individualChatMessage.SenderId].Name + "\": " + individualChatMessage.Content + "\r\n";
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
                    var chatContent = "[" + individualChatMessage.DateTime.ToString() + " " + individualChatMessage.SenderIp.ToString() + ":" + individualChatMessage.SenderPort + "]: \"" + client.participants[individualChatMessage.SenderId].Name + "\": " + individualChatMessage.Content + "\r\n";
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
            /*
             * 
             * 
             * отобразить тут событие для обработки сообщений с файлами
             * 
             * 
             */
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
                HandleChatMessage((CommonChatMessage)message);
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
            DownloadFileButton.Enabled = true;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            var clientUsername = "";
            if (ClientUsernameCheck(ref clientUsername))
            {
                var serverIndex = ServersListBox.SelectedIndex;
                if (client.ConnectToServer(serverIndex, clientUsername))
                {
                    EnableFormComponents();
                }
            }
        }

        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            if (FilesTempListBox.Items.Count == 0)
            {
                client.SendMessage(MessageTextBox.Text, selectedDialog);
            }
            else
            {
                client.SendFileMessage(MessageTextBox.Text, selectedDialog, fileSharingClient.filesToSendDictionary);
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
            var chatParticipant = client.participants[selectedDialog];
            if (UnreadMessageCheck(chatParticipant))
            {
                chatParticipant.SetUnreadMessageCountZero();
            }
            currentChatLabel.Text = client.participants[selectedDialog].Name;
            var newMessages = client.participants[selectedDialog].MessageHistory;
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

        private async void DownloadFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedFileIndex = FilesTempListBox.SelectedIndex;
                if (selectedFileIndex > -1 && selectedFileIndex < FilesTempListBox.Items.Count)
                {
                    var fileInfo = FilesTempListBox.Items[selectedFileIndex].ToString();
                    var fileId = fileSharingClient.GetFileIdByInfo(fileInfo);
                    if (fileId != -1)
                    {
                        var downloadFile = await fileSharingClient.DownloadFile(fileId, FileSharingServerUrl);
                        if (downloadFile != null)
                        {
                            var saveFileDialog = new SaveFileDialog();
                            saveFileDialog.FileName = downloadFile.FileName;
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                var filePath = saveFileDialog.FileName;
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    fileStream.Write(downloadFile.FileBytes, 0, downloadFile.FileBytes.Length);
                                }
                            }
                        }             
                    }
                    else
                    {
                        MessageBox.Show("Id файла с таким названием не найдено!", "Error!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Исключение: " + ex.Message);
            }
        }

        private async void DeleteFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedFileIndex = FilesTempListBox.SelectedIndex;
                if (selectedFileIndex > -1 && selectedFileIndex < FilesTempListBox.Items.Count)
                {
                    var fileInfo = FilesTempListBox.Items[selectedFileIndex].ToString();
                    var fileId = fileSharingClient.GetFileIdByInfo(fileInfo);
                    if (fileId != -1)
                    {
                        await fileSharingClient.DeleteFile(fileId, FileSharingServerUrl);
                    }
                    else
                    {
                        MessageBox.Show("Id файла с таким названием не найдено!", "Error!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Исключение: " + ex.Message);
            }
        }
    }
}
