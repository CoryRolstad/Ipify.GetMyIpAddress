using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ipify.GetMyIpAddress
{
    public class IpService : IIpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IpService> _logger;
        private readonly IpServiceSettings _ipServiceSettings; 

        public IpService(IHttpClientFactory httpClientFactory, ILogger<IpService> logger, IpServiceSettings ipServiceSettings)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException($"{nameof(httpClientFactory)} cannot be null");
            _logger = logger ?? throw new ArgumentNullException($"{nameof(logger)} cannot be null");
            if (ipServiceSettings == null)
                _ipServiceSettings = new IpServiceSettings();
            else
                _ipServiceSettings = ipServiceSettings;
        }

        public async Task<IPAddress> GetExternalIpv4()
        {
            _logger.LogInformation($"\t{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")}:\t\tGetExternalIpv4 Requested");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(_ipServiceSettings.IpV4Uri));
            string ipAddress = await MakeHttpRequest(request);
            return ParseIPAddressString(ipAddress);
        }

        public async Task<IPAddress> GetExternalIpv6()
        {
            _logger.LogInformation($"\t{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")}:\t\tGetExternalIpv6 Requested");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(_ipServiceSettings.Ipv6Uri));
            string ipAddress = await MakeHttpRequest(request);
            return ParseIPAddressString(ipAddress); 
        }

        private IPAddress ParseIPAddressString(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                throw new ArgumentException($"{nameof(ip)} cannot be Null or whitespace");
            ip = ip.Trim();             
            return IPAddress.Parse(ip);           
        }

        private async Task<string> MakeHttpRequest(HttpRequestMessage request)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.Timeout = new TimeSpan(0, 0, _ipServiceSettings.TimeOutInSeconds);
                HttpResponseMessage result = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                result.EnsureSuccessStatusCode();
                
                using (Stream responseStream = await result.Content.ReadAsStreamAsync())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        _logger.LogInformation($"\t{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")}:\t\t\tRequest completed Successfully");
                        string resultString = await streamReader.ReadToEndAsync();
                        return resultString;
                    }
                }
            }
        }

    }
}
