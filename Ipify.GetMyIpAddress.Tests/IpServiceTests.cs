using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ipify.GetMyIpAddress.Tests
{
    public class IpServiceTests
    {
        private readonly Mock<IServiceCollection> _serviceCollectionMock;
        private readonly Mock<ILogger<IpService>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IOptionsSnapshot<IpServiceSettings>> _options;
        private readonly string _resultTextString, _expectedIpTextReturnString;
        private readonly HttpResponseMessage _httpResponseMessage;
        private readonly IpServiceSettings _ipServiceSettings;

        public IpServiceTests()
        {
            _ipServiceSettings = new IpServiceSettings()
            {
                TimeOutInSeconds = 10
            };
            _serviceCollectionMock = new Mock<IServiceCollection>();
            _loggerMock = new Mock<ILogger<IpService>>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _options = new Mock<IOptionsSnapshot<IpServiceSettings>>();

            _resultTextString = "1.1.1.1";
            _expectedIpTextReturnString = "1.1.1.1";

            _httpResponseMessage = new HttpResponseMessage();
            _httpResponseMessage.StatusCode = HttpStatusCode.OK;
            _httpResponseMessage.Content = new StringContent(_resultTextString);
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

        [Fact]
        [Trait("Category", "IpService")]
        public async void GetIpv4_ReturnsExpectedString_ReturnsStringValue()
        {
            //Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Mock out the HttpMessageHandler to accept anything and return our private ResultString
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(_resultTextString)
                }).Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            _options.Setup(option => option.Get(It.IsAny<string>())).Returns(_ipServiceSettings);
            _httpClientFactoryMock.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient);
            HttpRequestMessage requestNullReference = new HttpRequestMessage();

            // Act
            IpService ipService = new IpService(_httpClientFactoryMock.Object, _loggerMock.Object, new IpServiceSettings());
            IPAddress actual = await ipService.GetExternalIpv4(); 

            //Assert    
            actual.ToString().Should().Be(_expectedIpTextReturnString);
        }


        [Fact]
        [Trait("Category", "IpService")]
        public async void GetIpv4_Returns500StatusCode_RaisesHttpRequestException()
        {
            //Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Mock out the HttpMessageHandler to accept anything and return our private ResultString
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(_resultTextString)
                }).Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            _options.Setup(option => option.Get(It.IsAny<string>())).Returns(_ipServiceSettings);
            _httpClientFactoryMock.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient);
            HttpRequestMessage requestNullReference = new HttpRequestMessage();

            // Act
            IpService ipService = new IpService(_httpClientFactoryMock.Object, _loggerMock.Object, new IpServiceSettings());
            // Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => ipService.GetExternalIpv4());                        
        }

        [Fact]
        [Trait("Category", "IpService")]
        public async void GetIpv6_ReturnsExpectedString_ReturnsStringValue()
        {
            //Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Mock out the HttpMessageHandler to accept anything and return our private ResultString
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(_resultTextString)
                }).Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            _options.Setup(option => option.Get(It.IsAny<string>())).Returns(_ipServiceSettings);
            _httpClientFactoryMock.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient);
            HttpRequestMessage requestNullReference = new HttpRequestMessage();

            // Act
            IpService ipService = new IpService(_httpClientFactoryMock.Object, _loggerMock.Object, new IpServiceSettings());
            IPAddress actual = await ipService.GetExternalIpv6();

            //Assert    
            actual.ToString().Should().Be(_expectedIpTextReturnString);
        }


        [Fact]
        [Trait("Category", "IpService")]
        public async void GetIpv6_Returns500StatusCode_RaisesHttpRequestException()
        {
            //Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Mock out the HttpMessageHandler to accept anything and return our private ResultString
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(_resultTextString)
                }).Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            _options.Setup(option => option.Get(It.IsAny<string>())).Returns(_ipServiceSettings);
            _httpClientFactoryMock.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient);
            HttpRequestMessage requestNullReference = new HttpRequestMessage();

            // Act
            IpService ipService = new IpService(_httpClientFactoryMock.Object, _loggerMock.Object, new IpServiceSettings());
            // Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => ipService.GetExternalIpv6());
        }
    }
}
