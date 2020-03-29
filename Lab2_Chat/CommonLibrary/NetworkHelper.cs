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
