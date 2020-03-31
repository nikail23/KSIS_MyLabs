﻿using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerProject
{
    internal class Server : IServer
    {
        private const int ServerPort = 50000;
        private const int ClientsLimit = 10;
        private const int CommonChatId = 0;
        private const string CommonChatName = "Common chat";
        private string name;
        private List<ClientHandler> clients;
        private List<Message> messageHistory;
        private Socket tcpSocket;
        private Socket udpSocket;
        private Thread listenUdpThread;
        private Thread listenTcpThread;
        private IMessageSerializer messageSerializer;

        public Server(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
            clients = new List<ClientHandler>();
            messageHistory = new List<Message>();
            listenUdpThread = new Thread(ListenUdp);
            listenTcpThread = new Thread(ListenTcp);
        }

        public void AddConnection()
        {

        }

        public void Close()
        {
            messageSerializer = null;
            messageHistory.Clear();
            messageHistory = null;
            clients.Clear();
            clients = null;
            CommonFunctions.CloseAndNullSocket(ref tcpSocket);
            CommonFunctions.CloseAndNullSocket(ref udpSocket);
            CommonFunctions.CloseAndNullThread(ref listenTcpThread);
            CommonFunctions.CloseAndNullThread(ref listenUdpThread);
        }

        public void ListenTcp()
        {
            int receivedDataBytesCount;
            byte[] receivedDataBuffer;
            tcpSocket.Listen(ClientsLimit);
            while (true)
            {
                try
                {
                    Socket connectedSocket = tcpSocket.Accept();
                    receivedDataBuffer = new byte[1024];
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        do
                        {
                            receivedDataBytesCount = connectedSocket.Receive(receivedDataBuffer, receivedDataBuffer.Length, SocketFlags.None);
                            memoryStream.Write(receivedDataBuffer, 0, receivedDataBytesCount);
                        }
                        while (udpSocket.Available > 0);
                        if (receivedDataBytesCount > 0)
                            HandleReceivedMessage(messageSerializer.Deserialize(memoryStream.ToArray()), connectedSocket);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
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
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private bool SetupUdpAndTcpLocalIp()
        {
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localUdpIp = new IPEndPoint(IPAddress.Any, ServerPort);
            IPEndPoint localTcpIp = new IPEndPoint(CommonFunctions.GetCurrrentHostIp(), ServerPort);
            try
            {
                udpSocket.Bind(localUdpIp);
                tcpSocket.Bind(localTcpIp);
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
            Console.Write("Server name: ");
            name = Console.ReadLine();
            if (SetupUdpAndTcpLocalIp())
            {
                WriteLine("Server is ready!");
                listenUdpThread.Start();
                listenTcpThread.Start();
            }
        }

        public void RemoveConnection(ClientHandler disconnectedClient)
        {
            if (clients.Remove(disconnectedClient))
                WriteLine("\"" + disconnectedClient.name + "\"" + " left from the chat!");
            SendMessageToAllClients(GetParticipantsListMessage());
            SendMessageToAllClients(GetMessagesHistoryMessage());
        }

        public void SendMessageToAllClients(Message message)
        {
            foreach (ClientHandler clientHandler in clients)
            {
                SendMessageToClient(message, clientHandler);
            }
        }

        public void SendMessageToClient(Message message, ClientHandler clientHandler)
        {
            clientHandler.tcpSocket.Send(messageSerializer.Serialize(message));
        }

        private void HandleClientUdpRequestMessage(ClientUdpRequestMessage clientUdpRequestMessage)
        {
            ServerUdpAnswerMessage serverUdpAnswerMessage = new ServerUdpAnswerMessage(DateTime.Now, CommonFunctions.GetCurrrentHostIp(), ServerPort, name);
            IPEndPoint clientEndPoint = new IPEndPoint(clientUdpRequestMessage.SenderIp, clientUdpRequestMessage.SenderPort);
            Socket serverUdpAnswerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverUdpAnswerSocket.SendTo(messageSerializer.Serialize(serverUdpAnswerMessage), clientEndPoint);
        }

        private void HandleRegistrationMessage(RegistrationMessage registrationMessage, Socket connectedSocket)
        {
            ClientHandler clientHandler = new ClientHandler(registrationMessage.ClientName, connectedSocket, GetClientUniqueId(), messageSerializer);
            clientHandler.ReceiveMessageEvent += HandleReceivedMessage;
            clientHandler.ClientDisconnectedEvent += RemoveConnection;
            clients.Add(clientHandler);
            clientHandler.StartListenTcp();
            IPEndPoint serverIp = (IPEndPoint)tcpSocket.LocalEndPoint;
            SendMessageToClient(new SendIdMessage(DateTime.Now, serverIp.Address, serverIp.Port, clientHandler.id), clientHandler);
            SendMessageToAllClients(GetParticipantsListMessage());
            SendMessageToAllClients(GetMessagesHistoryMessage());
        }

        private void HandleCommonChatMessage(CommonChatMessage commonChatMessage)
        {
            messageHistory.Add(commonChatMessage);
            SendMessageToAllClients(commonChatMessage);
            SendMessageToAllClients(GetMessagesHistoryMessage());
        }

        private void HandleIndividualChatMessage(IndividualChatMessage individualChatMessage)
        {
            foreach (ClientHandler clientHandler in clients)
            {
                if (clientHandler.id == individualChatMessage.ReceiverId)
                {
                    SendMessageToClient(individualChatMessage, clientHandler);
                    break;
                }
            }
        }

        public void HandleReceivedMessage(Message message)
        {
            if (message is ClientUdpRequestMessage)
            {
                ClientUdpRequestMessage clientUdpRequestMessage = (ClientUdpRequestMessage)message;
                WriteLine("Received a broadcast UDP request for connection info from " + clientUdpRequestMessage.SenderIp.ToString() + ":" + clientUdpRequestMessage.SenderPort);
                HandleClientUdpRequestMessage(clientUdpRequestMessage);
                WriteLine("Send UDP answer with server connection info to " + clientUdpRequestMessage.SenderIp.ToString() + ":" + clientUdpRequestMessage.SenderPort);
            }
            if (message is IndividualChatMessage)
            {
                IndividualChatMessage individualChatMessage = (IndividualChatMessage)message;
                HandleIndividualChatMessage(individualChatMessage);
            }
            else if (message is CommonChatMessage)
            {
                CommonChatMessage commonChatMessage = (CommonChatMessage)message;
                WriteLine("\"" + GetName(commonChatMessage.SenderId) + "\": " + commonChatMessage.Content);  // сделать name в client ( так в 100000 раз удобнее )
                HandleCommonChatMessage(commonChatMessage);            // пофиксить прием сообщений и вывод на клиенте, до сервака норм доходит
            }
        }

        public void HandleReceivedMessage(Message message, Socket connectedSocket)
        {
            if (message is RegistrationMessage)
            {
                RegistrationMessage registrationMessage = (RegistrationMessage)message;
                WriteLine("\"" + registrationMessage.ClientName + "\" has join the server");
                HandleRegistrationMessage(registrationMessage, connectedSocket);               
            }
        }

        private void WriteLine(string content)
        {
            Console.WriteLine("[" + DateTime.Now.ToString() + "]: " + content + ";");
        }

        private string GetName(int id)
        {
            foreach (ClientHandler clientHandler in clients)
            {
                if (id == clientHandler.id)
                {
                    return clientHandler.name;
                }
            }
            return null;
        }

        private MessagesHistoryMessage GetMessagesHistoryMessage()
        {
            IPEndPoint serverIp = (IPEndPoint)(tcpSocket.LocalEndPoint);
            return new MessagesHistoryMessage(DateTime.Now, serverIp.Address, serverIp.Port, messageHistory);
        }

        private ParticipantsListMessage GetParticipantsListMessage()
        {
            List<ChatParticipant> participantsList = new List<ChatParticipant>();
            participantsList.Add(new ChatParticipant() { name = CommonChatName, id = CommonChatId });
            foreach (ClientHandler clientHandler in clients)
            {
                participantsList.Add(new ChatParticipant() { name = clientHandler.name, id = clientHandler.id });
            }
            IPEndPoint serverIp = (IPEndPoint)(tcpSocket.LocalEndPoint);
            ParticipantsListMessage participantsListMessage = new ParticipantsListMessage(DateTime.Now, serverIp.Address, serverIp.Port, participantsList);
            return participantsListMessage;
        }

        private int GetClientUniqueId()
        {
            int clientsCount = 0;
            foreach (ClientHandler clientHandler in clients)
            {
                clientsCount++;
            }
            return clientsCount + 1;
        }
    }
}
