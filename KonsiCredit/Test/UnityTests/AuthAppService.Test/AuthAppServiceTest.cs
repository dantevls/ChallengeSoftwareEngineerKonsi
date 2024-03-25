using System.Net;
using Application.KonsiCredit.AuthViewModels;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Services.KonsiCredit.HttpClientHandler;
using Xunit;
using appService = Services.KonsiCredit.AuthAppService;

namespace AuthAppService.Test
{
    public class AuthAppServiceTest
    {
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private readonly Mock<IHttpClientHandler> _httpClientHandler = new Mock<IHttpClientHandler>();

        [Fact]
        public async Task GetUserToken_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var externalInssUri = "http://example.com"; // Sample URI
            var mockHttpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new ResponseTokenViewModel
                {
                    data = new Data
                    {
                        token = "mocked-token"
                    }
                }))
            };
            _httpClientHandler.Setup(client => client.PostAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(mockHttpResponse);

            _configurationMock.Setup(x => x.GetSection("ExternalInssApiUri").Value).Returns(externalInssUri);

            var authAppService = new appService.AuthAppService(_configurationMock.Object,
                _httpClientHandler.Object);

            // Act
            var result = await authAppService.GetUserToken(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("mocked-token", result);
        }

        // Add more test cases as needed
    }
}