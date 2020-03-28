using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerProject.Server
{
    class Server : IServer
    {
        const string BroadcastIp = "192.168.100.255";
        private Socket tcpSocketListener;
        private Socket udpSocketListener;

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
                    int receiveDataBytes = 0; 
                    byte[] dataBuffer = new byte[1024]; 
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
                    do
                    {
                        receiveDataBytes = udpSocketListener.ReceiveFrom(dataBuffer, ref remoteIp);
                        if (receiveDataBytes > 0)
                        {
                            // вызов десериализатора
                            // обработка сообщения
                        }
                    }
                    while (udpSocketListener.Available > 0);
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

        public void Open(string serverIp, int serverPort)
        {
            udpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint localIp = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            udpSocketListener.Bind(localIp);
            ListenUdp();
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
    }
}
