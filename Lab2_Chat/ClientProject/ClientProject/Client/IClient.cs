using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientProject
{
    public interface IClient
    {
        void SearchServers();
        void HandleReceivedMessage(Message message);
        void ListenTcp();
        void ListenUdp();
        void ConnectToServer(int serverIndex);
        void SendMessage(Message message);
        void DisconnectFromServer();
        void Close();
    }
}
