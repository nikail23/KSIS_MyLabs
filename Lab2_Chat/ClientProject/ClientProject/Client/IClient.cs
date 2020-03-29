using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProject
{
    public interface IClient
    {
        void SearchServers();
        void HandleReceivedMessage(Message message);
        void ListenTcp();
        void CloseTcp();
        void ListenUdp();
        void CloseUdp();
        void ConnectToServer(string serverIp, int serverPort);
        void SendMessage(Message message);
        void DisconnectFromServer();
    }
}
