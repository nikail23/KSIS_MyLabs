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
                string serverInfo = "Server: " + serverUdpAnswerMessage.senderIp.ToString() + ":" + serverUdpAnswerMessage.senderPort + ";";
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
    }
}
