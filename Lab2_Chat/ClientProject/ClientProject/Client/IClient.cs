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
        void ConnectToServer(int serverIndex, string clientName);
        void SendMessage(string content, int selectedDialog);
        void DisconnectFromServer();
        void Close();
    }
}
