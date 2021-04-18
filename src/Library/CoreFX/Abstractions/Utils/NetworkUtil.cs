using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace CoreFX.Abstractions.Utils
{
    public sealed class NetworkUtil
    {
        public static string GetLocalIPAddress()
        {
            if (null == _localIP)
            {
                try
                {
                    var firstUpInterface = NetworkInterface.GetAllNetworkInterfaces()
                        .OrderByDescending(c => c.Speed)
                        .FirstOrDefault(c =>
                            c.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                            c.OperationalStatus == OperationalStatus.Up
                        );

                    if (null != firstUpInterface)
                    {
                        var props = firstUpInterface.GetIPProperties();
                        _localIP = props.UnicastAddresses
                            .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                            .Select(c => c.Address)
                            .FirstOrDefault()?.ToString();
                    }

                    if (null == _localIP || AnyIP == _localIP)
                    {
                        // Might exception on linux
                        var host = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (var ip in host.AddressList)
                        {
                            if (AddressFamily.InterNetwork == ip.AddressFamily)
                            {
                                _localIP = ip.ToString();
                                break;
                            }
                        }
                    }
                }
                catch { }
            }

            if (null == _localIP)
            {
                _localIP = AnyIP;
            }

            return _localIP;
        }

        public const string AnyIP = "::1";
        private static string _localIP = null;
        public static string LocalIP
        {
            get
            {
                if (null != _localIP)
                {
                    return _localIP;
                }

                _localIP = GetLocalIPAddress();
                return _localIP;
            }
        }
    }
}
