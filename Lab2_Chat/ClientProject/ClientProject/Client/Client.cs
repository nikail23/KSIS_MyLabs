using CommonLibrary;
using System;
using System.Collections.Generic;
using System.IO;
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
        private const string BroadcastIp = "192.168.81.255";
        private const int ServerPort = 15000;
        private const int ClientPort = 7000;
        private List<ServerInfo> servers;
        private Socket tcpSocketListener;
        private Socket udpSocketListener;
        private Thread listenUdpThread;
        private Thread listenTcpThread;
        private IMessageSerializer messageSerializer;
        public delegate void ReceiveMessage(Message message);
        public event ReceiveMessage ReceiveMessageEvent;
        public Client(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
            servers = new List<ServerInfo>();
            tcpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            udpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            listenUdpThread = new Thread(ListenUdp);
            listenTcpThread = new Thread(ListenTcp);
        }

        public void ConnectToServer(string serverIp, int serverPort)
        {
            
        }

        public void DisconnectFromServer()
        {
            
        }

        private void AddNewServerInfo(ServerUdpAnswerMessage serverUdpAnswerMessage)
        {
            servers.Add(new ServerInfo(serverUdpAnswerMessage.senderIp, serverUdpAnswerMessage.senderPort));
        }

        public void HandleReceivedMessage(Message message)
        {
            if (message is ServerUdpAnswerMessage)
            {
                AddNewServerInfo((ServerUdpAnswerMessage)message);        
            }
            ReceiveMessageEvent(message);
        }

        public void ListenTcp()
        {
            
        }

        public void ListenUdp()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            EndPoint endPoint = ipEndPoint;
            int receivedDataBytesCount;
            byte[] receivedDataBuffer;
            while (true)
            {
                try
                {
                    receivedDataBuffer = new byte[1024];
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        do
                        {
                            receivedDataBytesCount = udpSocketListener.ReceiveFrom(receivedDataBuffer, receivedDataBuffer.Length, SocketFlags.None, ref endPoint);
                            memoryStream.Write(receivedDataBuffer, 0, receivedDataBytesCount);
                        }
                        while (udpSocketListener.Available > 0);
                        if (receivedDataBytesCount > 0)
                            HandleReceivedMessage(messageSerializer.Deserialize(memoryStream.ToArray()));
                    }
                }
                catch
                {

                }
            }
        }

        public void SearchServers()
        {
            udpSocketListener.Bind(new IPEndPoint(NetworkHelper.GetCurrrentHostIp(), ClientPort));
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Parse(BroadcastIp), ServerPort);  
            Socket sendUdpRequest = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint localIp = (IPEndPoint)udpSocketListener.LocalEndPoint;
            sendUdpRequest.SendTo(messageSerializer.Serialize(new ClientUdpRequestMessage(DateTime.Now, NetworkHelper.GetCurrrentHostIp(), localIp.Port)), broadcastEndPoint);
            listenUdpThread.Start();
        }

        public void SendMessage(Message message)
        {
            
        }

        public void Close()
        {
            if (tcpSocketListener != null)
            {
                tcpSocketListener.Close();
                tcpSocketListener = null;             
            }
            if (udpSocketListener != null)
            {
                udpSocketListener.Close();
                udpSocketListener = null;           
            }
            if (listenTcpThread != null)
            {
                listenTcpThread.Abort();
                listenTcpThread = null;
            }
            if (listenUdpThread != null)
            {
                listenUdpThread.Abort();
                listenUdpThread = null;
            }
        }

        ~Client()
        {
            Close();
        }
    }
}
