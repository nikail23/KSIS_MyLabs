using CommonLibrary;
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
            client.ReceiveMessageEvent += ShowReceivedMessage;
            client = new Client(new BinaryMessageSerializer());
            client.SearchServers();
        }

        private void AddServerInfoToServersListBox(ServerUdpAnswerMessage serverUdpAnswerMessage)
        {
            string serverInfo = "Server: " + serverUdpAnswerMessage.senderIp.ToString() + ":" + serverUdpAnswerMessage.senderPort + ";";
            serversListBox.Items.Add(serverInfo);
        }

        public void ShowReceivedMessage(Message message)
        {
            if (message is ServerUdpAnswerMessage)
            {
                AddServerInfoToServersListBox((ServerUdpAnswerMessage)message);
            }
        }
    }
}
