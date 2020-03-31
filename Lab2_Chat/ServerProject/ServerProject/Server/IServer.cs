using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProject
{
    public interface IServer
    {
        void Start();
        void ListenUdp();
        void ListenTcp();
        void HandleReceivedMessage(Message message);
        void SendMessageToClient(Message message, ClientHandler clientHandler);
        void SendMessageToAllClients(Message message);
        void AddConnection();
        void RemoveConnection(ClientHandler disconnectedClient);
        void Close();
    }
}
