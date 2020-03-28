using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProject
{
    class Client : IClient
    {
        private List<ServerInfo> servers;

        public Client()
        {
            servers = new List<ServerInfo>();
        }

        public void ConnectToServer(string serverIp, int serverPort)
        {
            
        }

        public void DisconnectFromServer()
        {
            
        }

        public void HandleReceivedMessage(Message message)
        {
            
        }

        public void ListenTcp()
        {
            
        }

        public void ListenUdp()
        {
            
        }

        public void SendMessage(Message message)
        {
            
        }
    }
}
