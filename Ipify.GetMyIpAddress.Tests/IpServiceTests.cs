using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace Ipify.GetMyIpAddress.Tests
{
    public class IpServiceTests
    {
        private readonly IpServiceSettings _webScannerSettings;
        private readonly Mock<IServiceCollection> _serviceCollectionMock;
        private readonly Mock<ILogger<IpService>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IOptionsSnapshot<IpServiceSettings>> _options;
        private readonly string _resultHtmlString, _resultXmlString, _xPathHtmlString, _xPathXmlString, _expectedHtmlXpathReturnString;
        private readonly HttpResponseMessage _httpResponseMessage, _xmlResponseMessage;
        private readonly HttpRequestMessage _httpRequestMessage;

        public IpServiceTests()
        {
            _webScannerSettings = new IpServiceSettings()
            {
                TimeOutInSeconds = 10
            };
            _serviceCollectionMock = new Mock<IServiceCollection>();
            _loggerMock = new Mock<ILogger<IpService>>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _options = new Mock<IOptionsSnapshot<IpServiceSettings>>();

            _resultHtmlString = "<!doctype html><html lang=\"en\"><head>  <meta charset=\"utf - 8\">  <title>The HTML5 Herald</title>  <meta name=\"description\" content=\"The HTML5 Herald\">  <meta name=\"author\" content=\"SitePoint\">  <link rel=\"stylesheet\" href=\"css / styles.css ? v = 1.0\"></head><body>  <script src=\"js / scripts.js\"></script>  <button type=\"button\">Button Text</button></body></html>";
            _resultXmlString = "";

            _xPathHtmlString = "/html/body/button";
            _xPathXmlString = "";

            _expectedHtmlXpathReturnString = "Button Text";

            _httpResponseMessage = new HttpResponseMessage();
            _httpResponseMessage.StatusCode = HttpStatusCode.OK;
            _httpResponseMessage.Content = new StringContent(_resultHtmlString);

            _httpResponseMessage = new HttpResponseMessage();
            _httpResponseMessage.StatusCode = HttpStatusCode.OK;
            _httpResponseMessage.Content = new StringContent(_resultXmlString);

            _httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://www.test.com");
        }

        [Fact]
        [Trait("Category", "IpServiceCollectionExtensions")]
        public void IpServiceCollectionExtensions_AddServiceWithNullSettingsValue_ThrowsArguementNullException()
        {
            //Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _serviceCollectionMock.Object.AddIpService(null);
            });
        }

        [Fact]
        [Trait("Category", "IpService")]
        public void IpService_InitializeNullHttpClientFactory_ThrowsArgumentNullException()
        {
            //Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                IpService ipService = new IpService(null, _loggerMock.Object, new IpServiceSettings());
            });
        }

        [Fact]
        [Trait("Category", "IpService")]
        public void IpService_InitializeNullLogger_ThrowsArgumentNullException()
        {
            //Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                IpService ipService = new IpService(_httpClientFactoryMock.Object, null, new IpServiceSettings());
            });
        }




    }
}
