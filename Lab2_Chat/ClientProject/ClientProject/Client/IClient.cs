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
        void HandleReceivedMessage(Message message);
        void ListenTcp();
        void ListenUdp();
        void ConnectToServer(string serverIp, int serverPort);
        void SendMessage(Message message);
        void DisconnectFromServer();
    }
}
