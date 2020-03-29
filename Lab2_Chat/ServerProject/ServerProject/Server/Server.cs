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

namespace ServerProject
{
    class Server : IServer
    {
        private const int ServerPort = 15000;
        const int ClientsLimit = 10;
        private Socket tcpSocketListener;
        private Socket udpSocketListener;
        Thread listenUdpThread;
        Thread listenTcpThread;
        private IMessageSerializer messageSerializer;

        public Server(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
            listenUdpThread = new Thread(ListenUdp);
            listenTcpThread = new Thread(ListenUdp);
        }

        public void AddConnection()
        {
            
        }

        public void Close()
        {
            
        }

        public void ListenTcp()
        {
            
        }

        public void ListenUdp()
        {
            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                EndPoint endPoint = ipEndPoint;
                while (true)
                {
                    int receivedDataBytesCount = 0;
                    byte[] dataBuffer = new byte[10000];//new byte[udpSocketListener.Available];
                    /*if (receiveDataBuffer.Length > 0)
                    {
                        Console.WriteLine("");
                    }*/
                    receivedDataBytesCount = udpSocketListener.ReceiveFrom(dataBuffer, ref endPoint);
                    if (receivedDataBytesCount > 0)
                        HandleReceivedMessage(messageSerializer.Deserialize(dataBuffer));
                }                            
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private bool SetupUdpAndTcpLocalIp()
        {
            udpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            tcpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localUdpIp = new IPEndPoint(NetworkHelper.GetCurrrentHostIp(), ServerPort);
            IPEndPoint localTcpIp = new IPEndPoint(NetworkHelper.GetCurrrentHostIp(), ServerPort);
            try
            {
                udpSocketListener.Bind(localUdpIp);              
                tcpSocketListener.Bind(localTcpIp);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
            
        }

        public void Start()
        {
            if (SetupUdpAndTcpLocalIp())
            {
                Console.WriteLine("Server is ready to listen Tcp and Udp request's!");
                listenUdpThread.Start();
                listenTcpThread.Start();
            }        
        }

        public void RemoveConnection()
        {
            
        }

        public void SendMessageToAllClients()
        {
            
        }

        public void SendMessageToClient()
        {
            
        }

        private void HandleClientUdpRequestMessage(ClientUdpRequestMessage clientUdpRequestMessage)
        {
            ServerUdpAnswerMessage serverUdpAnswerMessage = new ServerUdpAnswerMessage(DateTime.Now, NetworkHelper.GetCurrrentHostIp(), ServerPort);
            IPEndPoint clientEndPoint = new IPEndPoint(clientUdpRequestMessage.senderIp, clientUdpRequestMessage.senderPort);
            Socket serverUdpAnswerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverUdpAnswerSocket.SendTo(messageSerializer.Serialize(serverUdpAnswerMessage), clientEndPoint);
        }

        public void HandleReceivedMessage(Message message)
        {
            if (message is ClientUdpRequestMessage)
            {
                HandleClientUdpRequestMessage((ClientUdpRequestMessage)message);
            }
        }
    }
}
