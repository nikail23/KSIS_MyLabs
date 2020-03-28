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
        private const string ServerIp = "127.0.0.1";
        const int ClientsLimit = 10;
        private Socket tcpSocketListener;
        private Socket udpSocketListener;
        private IMessageSerializer messageSerializer;

        public Server(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
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
                while (true)
                {
                    int receivedDataBytesCount = 0;
                    byte[] receiveDataBuffer = new byte[1024];
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, ServerPort);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        do
                        {
                            receivedDataBytesCount = udpSocketListener.ReceiveFrom(receiveDataBuffer, ref remoteIp);
                            memoryStream.Write(receiveDataBuffer, 0, receivedDataBytesCount);
                        } 
                        while (udpSocketListener.Available > 0);
                        HandleReceivedMessage(messageSerializer.Deserialize(memoryStream.ToArray()));
                    }
                }                            
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                Close();
            }
        }

        private bool SetupUdpAndTcpLocalIp()
        {
            udpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            tcpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localUdpIp = new IPEndPoint(IPAddress.Any, 0);
            IPEndPoint localTcpIp = new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort);
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
                Thread listenUdpThread = new Thread(ListenUdp);
                Thread listenTcpThread = new Thread(ListenTcp);
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
            IPEndPoint tcpSocketListenerEndPoint = (IPEndPoint)tcpSocketListener.RemoteEndPoint;
            ServerUdpAnswerMessage serverUdpAnswerMessage = new ServerUdpAnswerMessage(DateTime.Now, tcpSocketListenerEndPoint.Address.ToString(), ServerPort);
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Parse(clientUdpRequestMessage.senderIp), clientUdpRequestMessage.senderPort);
            Socket serverUdpAnswerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverUdpAnswerSocket.Bind(clientEndPoint);
            serverUdpAnswerSocket.Send(messageSerializer.Serialize(serverUdpAnswerMessage));
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
