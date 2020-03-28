using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientProject
{
    class Client : IClient
    {
        private const string ClientIp = "127.0.0.1";
        private const int ClientPort = 15000;
        private const string BroadcastIp = "255.255.255.255";
        private const int ServerPort = 15000;
        private List<ServerInfo> servers;
        private Socket tcpSocketListener;
        private Socket udpSocketListener;
        private IMessageSerializer messageSerializer;

        public Client(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
            servers = new List<ServerInfo>();
            tcpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            udpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            SearchServers();
        }

        public void ConnectToServer(string serverIp, int serverPort)
        {
            
        }

        public void DisconnectFromServer()
        {
            
        }

        public void HandleReceivedMessage(Message message)
        {
            if (message is ServerUdpAnswerMessage)
            {
                // тут продолжить
            }
        }

        public void ListenTcp()
        {
            
        }

        public void ListenUdp()
        {
            
        }

        public void SearchServers()
        {
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Parse(BroadcastIp), ServerPort);
            udpSocketListener.Bind(broadcastEndPoint);
            udpSocketListener.Send(messageSerializer.Serialize(new ClientUdpRequestMessage(DateTime.Now, ClientIp, ClientPort)));
            Thread threadListenUdp = new Thread(ListenUdp);
            threadListenUdp.Start();
        }

        public void SendMessage(Message message)
        {
            
        }
    }
}
