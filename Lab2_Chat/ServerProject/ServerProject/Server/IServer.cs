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
        void Start(string serverIp, int serverPort);
        void ListenUdp();
        void ListenTcp();
        void HandleReceivedMessage(Message message);
        void SendMessageToClient();
        void SendMessageToAllClients();
        void AddConnection();
        void RemoveConnection();
        void Close();
    }
}
