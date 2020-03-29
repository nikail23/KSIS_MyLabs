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
        private const int ClientPort = 15000;
        private const string BroadcastIp = "255.255.255.255";
        private const int ServerPort = 15000;
        private List<ServerInfo> servers;
        private Socket tcpSocketListener;
        private Socket udpSocketListener;
        private IMessageSerializer messageSerializer;
        public delegate void ReceiveMessage(Message message);
        public event ReceiveMessage ReceiveMessageEvent;
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
            try
            {
                while (true)
                {
                    int bytesCount = 0; 
                    byte[] dataBuffer = new byte[256]; 
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        do
                        {
                            bytesCount = udpSocketListener.ReceiveFrom(dataBuffer, ref remoteIp);
                            memoryStream.Write(dataBuffer, 0, bytesCount);
                        }
                        while (udpSocketListener.Available > 0);
                        HandleReceivedMessage(messageSerializer.Deserialize(memoryStream.ToArray()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseUdp();
            }
        }

        public void SearchServers()
        {
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Any, 0);  // почему не Broadcast
            udpSocketListener.Bind(broadcastEndPoint);
            udpSocketListener.Send(messageSerializer.Serialize(new ClientUdpRequestMessage(DateTime.Now, NetworkHelper.GetCurrrentHostIp(), ClientPort)));
            Thread threadListenUdp = new Thread(ListenUdp);
            threadListenUdp.Start();
        }

        public void SendMessage(Message message)
        {
            
        }

        public void CloseTcp()
        {
            if (tcpSocketListener != null)
            {
                tcpSocketListener.Shutdown(SocketShutdown.Both);
                tcpSocketListener.Close();
                tcpSocketListener = null;
            }
        }

        public void CloseUdp()
        {
            if (udpSocketListener != null)
            {
                udpSocketListener.Shutdown(SocketShutdown.Both);
                udpSocketListener.Close();
                udpSocketListener = null;
            }
        }
    }
}
