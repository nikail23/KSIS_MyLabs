using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public static class NetworkHelper
    {
        public static IPAddress GetCurrrentHostIp()
        {
            string host = Dns.GetHostName();
            return Dns.GetHostEntry(host).AddressList[0];
        }
    }
}
