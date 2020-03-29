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

        private void AddServerInfoToServersListBox(Message message)
        {
            string serverInfo = "Server: " + message.senderIp.ToString() + ":" + message.senderPort + ";";
            serversListBox.Items.Add(serverInfo);
        }


        public void ShowReceivedMessage(Message message)
        {

            if (message is ServerUdpAnswerMessage)
            {
                Action action = delegate
                {
                    AddServerInfoToServersListBox(message);
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
        }
    }
}
