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
        const string BroadcastIp = "192.168.100.255";
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
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
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

        private Boolean SetupUdpAndTcpLocalIp(string serverIp, int serverPort)
        {
            udpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            tcpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localIp = new IPEndPoint(IPAddress.Any, serverPort);
            try
            {
                udpSocketListener.Bind(localIp);
                tcpSocketListener.Bind(localIp);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
            
        }

        public void Start(string serverIp, int serverPort)
        {
            if (SetupUdpAndTcpLocalIp(serverIp, serverPort))
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

        public void HandleReceivedMessage(Message message)
        {
            
        }
    }
}
