using System;

namespace Ipify.GetMyIpAddress
{
    public class IpServiceSettings
    {
        public int TimeOutInSeconds { get; set; } = 10; 

        public string IpV4Uri { get; set; } = "https://api.ipify.org";
        public string Ipv6Uri { get; set; } = "https://api64.ipify.org";
    }
}
