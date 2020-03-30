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
        private List<ServerInfo> serversInfo;
        private Socket tcpSocket;
        private Socket udpSocket;
        private Thread listenUdpThread;
        private Thread listenTcpThread;
        private IMessageSerializer messageSerializer;
        public delegate void ReceiveMessage(Message message);
        public event ReceiveMessage ReceiveMessageEvent;
        public Client(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
            serversInfo = new List<ServerInfo>();
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            listenUdpThread = new Thread(ListenUdp);
            listenTcpThread = new Thread(ListenTcp);
        }

        public void ConnectToServer(int serverIndex)
        {
            try
            {
                if ((serverIndex >= 0) && (serverIndex <= serversInfo.Count - 1))
                {
                    ServerInfo serverInfo = GetServerInfo(serverIndex);
                    IPEndPoint clientIp = new IPEndPoint(serverInfo.ServerIp, serverInfo.ServerPort);
                    tcpSocket.Connect(clientIp);
                }
            }
            catch 
            {
                Close();
            }
                 
        }

        public void DisconnectFromServer()
        {
            
        }

        private void AddNewServerInfo(ServerUdpAnswerMessage serverUdpAnswerMessage)
        {
            serversInfo.Add(new ServerInfo(serverUdpAnswerMessage.senderIp, serverUdpAnswerMessage.senderPort));
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
                            receivedDataBytesCount = udpSocket.ReceiveFrom(receivedDataBuffer, receivedDataBuffer.Length, SocketFlags.None, ref endPoint);
                            memoryStream.Write(receivedDataBuffer, 0, receivedDataBytesCount);
                        }
                        while (udpSocket.Available > 0);
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
            udpSocket.Bind(new IPEndPoint(CommonFunctions.GetCurrrentHostIp(), ClientPort));
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Parse(BroadcastIp), ServerPort);  
            Socket sendUdpRequest = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint localIp = (IPEndPoint)udpSocket.LocalEndPoint;
            sendUdpRequest.SendTo(messageSerializer.Serialize(new ClientUdpRequestMessage(DateTime.Now, CommonFunctions.GetCurrrentHostIp(), localIp.Port)), broadcastEndPoint);
            listenUdpThread.Start();
        }

        public void SendMessage(Message message)
        {
            
        }

        public void Close()
        {
            CommonFunctions.CloseAndNullSocket(ref tcpSocket);
            CommonFunctions.CloseAndNullSocket(ref udpSocket);
            CommonFunctions.CloseAndNullThread(ref listenTcpThread);
            CommonFunctions.CloseAndNullThread(ref listenUdpThread);
        }

        private ServerInfo GetServerInfo(int serverIndex)
        {
            if ((serverIndex >= 0)&&(serverIndex <= serversInfo.Count - 1))
            {
                return serversInfo[serverIndex];
            }
            return null;
        }

        ~Client()
        {
            Close();
        }
    }
}
