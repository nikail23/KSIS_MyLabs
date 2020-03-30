using CommonLibrary;
using System;
using System.Windows.Forms;
using Message = CommonLibrary.Message;

namespace ClientProject
{
    public partial class mainForm : Form
    {
        private Client client;

        public mainForm()
        {
            InitializeComponent();
            client = new Client(new BinaryMessageSerializer());
            client.ReceiveMessageEvent += ShowReceivedMessage;
            client.SearchServers();
        }

        private void AddServerInfoToServersListBox(ServerUdpAnswerMessage serverUdpAnswerMessage)
        {
            Action action = delegate
            {
                string serverInfo = serverUdpAnswerMessage.serverName;
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


        public void ShowReceivedMessage(Message message)
        {

            if (message is ServerUdpAnswerMessage)
            {
                AddServerInfoToServersListBox((ServerUdpAnswerMessage)message);
            }
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Close();
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //client.SendMessage();
        }

        private bool ClientUsernameCheck(string clientUsername)
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
            if (ClientUsernameCheck(clientUsername))
            {
                int serverIndex = serversListBox.SelectedIndex;
                client.ConnectToServer(serverIndex, clientUsername);
            }
        }
    }
}
