using System.Net;
using System.Net.Sockets;

namespace CoreFX.Common.Utils
{
    public sealed class NetworkUtil
    {
        public static string GetIPAddress()
        {
            var str = "?";
            var addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            for (var i = 0; i < addressList.Length; i++)
            {
                var pAddress = addressList[i];
                if (pAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    str = pAddress.ToString();
                }
            }
            return str;
        }
    }
}
