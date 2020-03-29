using CommonLibrary;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerProject
{
    internal class Server : IServer
    {
        private const int ServerPort = 15000;
        private const int ClientsLimit = 10;
        private Socket tcpSocketListener;
        private Socket udpSocketListener;
        private Thread listenUdpThread;
        private Thread listenTcpThread;
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
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Close();
                }
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

        ~Server()
        {
            Close();
        }
    }
}
