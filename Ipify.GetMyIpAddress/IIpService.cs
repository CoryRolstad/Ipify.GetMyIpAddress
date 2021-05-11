using System;
using System.Net;
using System.Threading.Tasks;

namespace Ipify.GetMyIpAddress
{
    public interface IIpService
    {
        /// <summary>
        /// GetExternalIpv4 Gets your external ipv4 address from ipify
        /// </summary>
        /// <returns>IPAddress</returns>
        public Task<string> GetExternalIpv4();

        /// <summary>
        /// GetExternalIpv4 Gets your external ipv6 address from ipify
        /// </summary>
        /// <returns>IPAddress</returns>
        public Task<string> GetExternalIpv6(); 

    }
}
