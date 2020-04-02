using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerProject
{
    public class ClientHandler
    {
        public string name;
        public Socket tcpSocket;
        public int id;
        private Thread listenTcpThread;
        private IMessageSerializer messageSerializer;
        public delegate void ReceiveMessageDelegate(Message message);
        public event ReceiveMessageDelegate ReceiveMessageEvent;
        public delegate void ClientDisconnected(ClientHandler clientHandler);
        public event ClientDisconnected ClientDisconnectedEvent;

        public ClientHandler(string name, Socket socket, int id, IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
            this.name = name;
            this.id = id;
            tcpSocket = socket;
            listenTcpThread = new Thread(ListenTcp);
        }

        public void StartListenTcp()
        {
            listenTcpThread.Start();
        }

        private void ListenTcp()
        {
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
                            receivedDataBytesCount = tcpSocket.Receive(receivedDataBuffer, receivedDataBuffer.Length, SocketFlags.None);
                            memoryStream.Write(receivedDataBuffer, 0, receivedDataBytesCount);
                        }
                        while (tcpSocket.Available > 0);
                        if (receivedDataBytesCount > 0)
                            ReceiveMessageEvent(messageSerializer.Deserialize(memoryStream.ToArray()));
                    }
                }
                catch (SocketException)
                {
                    ClientDisconnectedEvent(this);
                    CommonFunctions.CloseAndNullSocket(ref tcpSocket);
                    CommonFunctions.CloseAndNullThread(ref listenTcpThread);            
                }
            }
        }
    }
}
