using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProject.Server
{
    public interface IServer
    {
        void Open();
        void ListenUdp();
        void ListenTcp();
        void SendMessageToClient();
        void SendMessageToAllClients();
        void AddConnection();
        void RemoveConnection();
        void Close();
    }
}
