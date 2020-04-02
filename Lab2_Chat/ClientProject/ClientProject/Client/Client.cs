using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientProject
{
    public delegate void ReceiveMessage(Message message);

    class Client 
    {
        private const int ServerPort = 50000;
        public int id;
        public string name
        {
            get
            {
                foreach (ChatParticipant chatParticipant in participants)
                {
                    if (id == chatParticipant.Id)
                    {
                        return chatParticipant.Name;
                    }
                }
                return null;
            }
        }
        private List<ServerInfo> serversInfo;
        public List<ChatParticipant> participants;
        private Socket tcpSocket;
        private Socket udpSocket;
        private Thread listenUdpThread;
        private Thread listenTcpThread;
        private IMessageSerializer messageSerializer;
        public event ReceiveMessage ReceiveMessageEvent;
        public event UnreadMessageDelegate UnreadMessageEvent;
        public event ReadMessage ReadMessageEvent;

        public Client(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
            serversInfo = new List<ServerInfo>();
            participants = new List<ChatParticipant>();
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.EnableBroadcast = true;
            listenUdpThread = new Thread(ListenUdp);
            listenTcpThread = new Thread(ListenTcp);
        }

        public void ConnectToServer(int serverIndex, string clientName)
        {
            try
            {
                if ((serverIndex >= 0) && (serverIndex <= serversInfo.Count - 1))
                {
                    tcpSocket.Connect(GetServerIpEndPoint(serverIndex));
                    listenTcpThread.Start();                   
                    SendMessage(GetRegistrationMessage(clientName));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Close();
            }                
        }

        public void DisconnectFromServer()
        {
            CommonFunctions.CloseAndNullSocket(ref tcpSocket);
            CommonFunctions.CloseAndNullThread(ref listenTcpThread);
        }

        private void AddNewServerInfo(ServerUdpAnswerMessage serverUdpAnswerMessage)
        {
            serversInfo.Add(new ServerInfo(serverUdpAnswerMessage.ServerName, serverUdpAnswerMessage.SenderIp, serverUdpAnswerMessage.SenderPort));
        }

        private void SetEventsForParticipants()
        {
            foreach (ChatParticipant chatParticipant in participants)
            {
                chatParticipant.UnreadMessageEvent += UnreadMessageEvent;
                chatParticipant.ReadMessageEvent += ReadMessageEvent;
            }
        }

        private void HandleParticipantsListMessage(ParticipantsListMessage participantsListMessage) 
        {
            participants = participantsListMessage.participants;
            SetEventsForParticipants();
        }

        private void HandleMessagesHistoryMessage(MessagesHistoryMessage messagesHistoryMessage)
        {
            if (participants.Count != 0)
                participants[0].MessageHistory = messagesHistoryMessage.MessagesHistory;
        }

        private void HandleIndividualChatMessage(IndividualChatMessage individualChatMessage)
        {
            foreach (ChatParticipant chatParticipant in participants) 
            {
                if (chatParticipant.Id == individualChatMessage.SenderId)
                {
                    chatParticipant.MessageHistory.Add(individualChatMessage);
                    chatParticipant.UnreadMessagesCountIncrement();
                    break;
                }
            }
        }

        public void HandleReceivedMessage(Message message)
        {
            if (message is ServerUdpAnswerMessage)
            {
                AddNewServerInfo((ServerUdpAnswerMessage)message);        
            }
            if (message is ParticipantsListMessage)
            {
                HandleParticipantsListMessage((ParticipantsListMessage)message); 
            }
            if (message is MessagesHistoryMessage)
            {
                HandleMessagesHistoryMessage((MessagesHistoryMessage)message);
                return;
            }
            if (message is IndividualChatMessage)
            {
                HandleIndividualChatMessage((IndividualChatMessage)message);
            }
            if (message is SendIdMessage)
            {
                id = ((SendIdMessage)message).Id;
                return;
            }
            ReceiveMessageEvent(message);
        }

        public void ListenTcp()
        {
            int receivedDataBytesCount;
            byte[] receivedDataBuffer;
            try
            {
                while (true)
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
                            HandleReceivedMessage(messageSerializer.Deserialize(memoryStream.ToArray()));
                    }
                }              
            }
            catch (SocketException)
            {
                DisconnectFromServer();
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
                catch (SocketException)
                {
                    Close();
                }
            }
        }

        public void SearchServers()
        {
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, ServerPort);
            IPEndPoint localIp = new IPEndPoint(IPAddress.Any, 0);
            udpSocket.Bind(localIp);
            Socket sendUdpRequest = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sendUdpRequest.EnableBroadcast = true;
            sendUdpRequest.SendTo(messageSerializer.Serialize(GetClientUdpRequestMessage()), broadcastEndPoint);
            listenUdpThread.Start();
        }

        public void SendMessage(string content, int selectedDialog)
        {
            if (participants[selectedDialog].Id == 0)
            {        
                tcpSocket.Send(messageSerializer.Serialize(GetCommonChatMessage(content)));
            }
            else
            {
                IndividualChatMessage individualChatMessage = GetIndividualChatMessage(content, participants[selectedDialog].Id);
                if (individualChatMessage.SenderId != individualChatMessage.ReceiverId)
                {
                    tcpSocket.Send(messageSerializer.Serialize(individualChatMessage));
                }
                participants[selectedDialog].MessageHistory.Add(individualChatMessage);
                ReceiveMessageEvent(individualChatMessage);
            }
        }

        public void SendMessage(Message message)
        {
            tcpSocket.Send(messageSerializer.Serialize(message));
        }

        public void Close()
        {
            CommonFunctions.CloseAndNullSocket(ref tcpSocket);
            CommonFunctions.CloseAndNullSocket(ref udpSocket);
            CommonFunctions.CloseAndNullThread(ref listenTcpThread);
            CommonFunctions.CloseAndNullThread(ref listenUdpThread);
        }

        private IndividualChatMessage GetIndividualChatMessage(string content, int receiverId)
        {
            IPEndPoint clientIp = (IPEndPoint)(tcpSocket.LocalEndPoint);
            return new IndividualChatMessage(DateTime.Now, clientIp.Address, clientIp.Port, content, id, receiverId);
        }

        private CommonChatMessage GetCommonChatMessage(string content)
        {
            IPEndPoint clientIp = (IPEndPoint)(tcpSocket.LocalEndPoint);
            return new CommonChatMessage(DateTime.Now, clientIp.Address, clientIp.Port, content, id);
        }

        private ClientUdpRequestMessage GetClientUdpRequestMessage()
        {
            IPEndPoint localIp = (IPEndPoint)udpSocket.LocalEndPoint;
            return new ClientUdpRequestMessage(DateTime.Now, CommonFunctions.GetCurrrentHostIp(), localIp.Port);
        }

        private IPEndPoint GetServerIpEndPoint(int serverIndex)
        {
            ServerInfo serverInfo = GetServerInfo(serverIndex);
            return new IPEndPoint(serverInfo.ServerIp, serverInfo.ServerPort);
        }

        private RegistrationMessage GetRegistrationMessage(string clientName)
        {
            IPEndPoint clientIp = (IPEndPoint)(tcpSocket.LocalEndPoint);
            return new RegistrationMessage(DateTime.Now, clientIp.Address, clientIp.Port, clientName);
        }

        private ServerInfo GetServerInfo(int serverIndex)
        {
            if ((serverIndex >= 0)&&(serverIndex <= serversInfo.Count - 1))
            {
                return serversInfo[serverIndex];
            }
            return null;
        }
    }
}
