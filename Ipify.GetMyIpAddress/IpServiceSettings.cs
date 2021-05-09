using System;

namespace Ipify.GetMyIpAddress
{
    public class IpServiceSettings
    {
        public int TimeOutInSeconds { get; set; } = 10; 

        public string IpV4Uri { get; set; } = "api.ipify.org";
        public string Ipv6Uri { get; set; } = "api64.ipify.org";
    }
}
