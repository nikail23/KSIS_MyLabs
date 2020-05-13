using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerProject
{
    internal class Server
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

        public void Close()
        {
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
                    var connectedSocket = tcpSocket.Accept();
                    receivedDataBuffer = new byte[1024];
                    using (var memoryStream = new MemoryStream())
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
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var endPoint = (EndPoint)ipEndPoint;
            int receivedDataBytesCount;
            byte[] receivedDataBuffer;
            while (true)
            {
                try
                {
                    receivedDataBuffer = new byte[1024];
                    using (var memoryStream = new MemoryStream())
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
            udpSocket.EnableBroadcast = true;
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var localUdpIp = new IPEndPoint(IPAddress.Any, ServerPort);
            var localTcpIp = new IPEndPoint(CommonFunctions.GetCurrrentHostIp(), ServerPort);
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
        }

        public void SendMessageToAllClients(Message message)
        {
            foreach (var clientHandler in clients)
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
            var serverUdpAnswerMessage = GetServerUdpAnswerMessage();
            var clientEndPoint = new IPEndPoint(clientUdpRequestMessage.SenderIp, clientUdpRequestMessage.SenderPort);
            var serverUdpAnswerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverUdpAnswerSocket.SendTo(messageSerializer.Serialize(serverUdpAnswerMessage), clientEndPoint);
        }

        private ClientHandler GetClientHandler(RegistrationMessage registrationMessage, Socket connectedSocket)
        {
            var clientHandler = new ClientHandler(registrationMessage.ClientName, connectedSocket, GetClientUniqueId(), messageSerializer);
            clientHandler.ReceiveMessageEvent += HandleReceivedMessage;
            clientHandler.ClientDisconnectedEvent += RemoveConnection;
            clients.Add(clientHandler);
            clientHandler.StartListenTcp();
            return clientHandler;
        }

        private void HandleRegistrationMessage(RegistrationMessage registrationMessage, Socket connectedSocket)
        {
            var clientHandler = GetClientHandler(registrationMessage, connectedSocket);
            SendMessageToClient(GetSendIdMessage(clientHandler), clientHandler);
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
            foreach (var clientHandler in clients)
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
                var clientUdpRequestMessage = (ClientUdpRequestMessage)message;               
                HandleClientUdpRequestMessage(clientUdpRequestMessage);              
            }
            if (message is IndividualChatMessage)
            {
                var individualChatMessage = (IndividualChatMessage)message;
                HandleIndividualChatMessage(individualChatMessage);
            }
            else if (message is CommonChatMessage)
            {
                var commonChatMessage = (CommonChatMessage)message;
                WriteLine("\"" + GetName(commonChatMessage.SenderId) + "\": " + commonChatMessage.Content);  
                HandleCommonChatMessage(commonChatMessage);            
            }
        }

        public void HandleReceivedMessage(Message message, Socket connectedSocket)
        {
            if (message is RegistrationMessage)
            {
                var registrationMessage = (RegistrationMessage)message;
                WriteLine("\"" + registrationMessage.ClientName + "\" has join the server");
                HandleRegistrationMessage(registrationMessage, connectedSocket);               
            }
        }

        private string GetName(int id)
        {
            foreach (var clientHandler in clients)
            {
                if (id == clientHandler.id)
                {
                    return clientHandler.name;
                }
            }
            return null;
        }

        private void WriteLine(string content)
        {
            Console.WriteLine("[" + DateTime.Now.ToString() + "]: " + content + ";");
        }

        private ServerUdpAnswerMessage GetServerUdpAnswerMessage() 
        {
            return new ServerUdpAnswerMessage(DateTime.Now, CommonFunctions.GetCurrrentHostIp(), ServerPort, name);
        }

        private SendIdMessage GetSendIdMessage(ClientHandler clientHandler)
        {
            var serverIp = (IPEndPoint)tcpSocket.LocalEndPoint;
            return new SendIdMessage(DateTime.Now, serverIp.Address, serverIp.Port, clientHandler.id);
        }

        private MessagesHistoryMessage GetMessagesHistoryMessage()
        {
            var serverIp = (IPEndPoint)(tcpSocket.LocalEndPoint);
            return new MessagesHistoryMessage(DateTime.Now, serverIp.Address, serverIp.Port, messageHistory);
        }

        private ParticipantsListMessage GetParticipantsListMessage()
        {
            var participantsList = new List<ChatParticipant>();
            participantsList.Add(new ChatParticipant(CommonChatName, CommonChatId, new List<Message>(), new Dictionary<int, string>()));
            foreach (var clientHandler in clients)
            {
                participantsList.Add(new ChatParticipant(clientHandler.name, clientHandler.id, new List<Message>(), new Dictionary<int, string>()));
            }
            var serverIp = (IPEndPoint)(tcpSocket.LocalEndPoint);
            var participantsListMessage = new ParticipantsListMessage(DateTime.Now, serverIp.Address, serverIp.Port, participantsList);
            return participantsListMessage;
        }

        private int GetClientUniqueId()
        {
            var clientsCount = 0;
            foreach (var clientHandler in clients)
            {
                clientsCount++;
            }
            return clientsCount + 1;
        }

        ~Server()
        {
            Close();
        }
    }
}
