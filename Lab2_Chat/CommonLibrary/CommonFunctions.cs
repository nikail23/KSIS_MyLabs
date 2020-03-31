using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [Serializable]
    public struct ChatParticipant
    {
        public string name;
        public int id;
        public List<Message> messageHistory;
    }

    public static class CommonFunctions
    {
        public static void CloseAndNullSocket(ref Socket socket)
        {
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
        }

        public static void CloseAndNullThread(ref Thread thread)
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        public static IPAddress GetCurrrentHostIp()
        {
            string host = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostEntry(host).AddressList;
            foreach (var address in addresses)
            {
                if (address.GetAddressBytes().Length == 4)
                {
                    return address;
                }
            }
            return null;
        }
    }
}
