using System.Net;
using Application.KonsiCredit.UserBenefitsViewModels;
using Infra.Data.KonsiCredit.CachingRepository;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Services.KonsiCredit.AuthAppService;
using Services.KonsiCredit.ElasticSearchAppService;
using Services.KonsiCredit.HttpClientHandler;
using Xunit;

namespace BenefitsAppService.Test
{
    public class BenefitsAppServiceTest
    {
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private readonly Mock<ICachingRepository> _cacheMock = new Mock<ICachingRepository>();
        private readonly Mock<IElasticSearchAppService> _elasticSearchMock = new Mock<IElasticSearchAppService>();
        private readonly Mock<IAuthAppService> _authAppServiceMock = new Mock<IAuthAppService>();
        private readonly Mock<IHttpClientHandler> _httpClientHandler = new Mock<IHttpClientHandler>();
        private readonly Services.KonsiCredit.BenefitsAppService.BenefitsAppService _benefitsAppService;
        
        public BenefitsAppServiceTest()
        {
                 _benefitsAppService = new Services.KonsiCredit.BenefitsAppService.BenefitsAppService(
                _configurationMock.Object,
                _cacheMock.Object,
                _elasticSearchMock.Object,
                _authAppServiceMock.Object,
                _httpClientHandler.Object
            );
        }
        
        [Fact]
        public async Task GetUserBenefits_ValidCpf_ReturnsUserBenefitsViewModel()
        {
            // Arrange
            var cpf = "12345678900"; // Valid CPF
            var externalInssUri = "http://example.com";
            
            var mockHttpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new UserBenefitsViewModel(new Data(cpf: cpf, 
                    new Benefits[]{}))))
            };
            _httpClientHandler
                .Setup(client => client.GetAsync(It.IsAny<HttpRequestMessage>(),It.IsAny<string>()))
                .ReturnsAsync(mockHttpResponse);

            _configurationMock.Setup(x => x.GetSection("ExternalInssApiUri").Value).Returns(externalInssUri);
            _configurationMock.Setup(x => x.GetSection("UserRoot").Value).Returns("userRoot");
            _configurationMock.Setup(x => x.GetSection("PassRoot").Value).Returns("passRoot");
            _authAppServiceMock.Setup(x => x.GetUserToken(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("mocked-token");

            // Act
            var result = await _benefitsAppService.GetUserBenefits(cpf);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserBenefitsViewModel>(result);
        }

        [Fact]
        public async Task GetUserBenefits_InvalidCpf_ReturnsEmptyUserBenefitsViewModel()
        {
            // Arrange
            var cpf = ""; // Invalid CPF

            // Act
            var result = await _benefitsAppService.GetUserBenefits(cpf);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserBenefitsViewModel>(result);
            Assert.Null(result.data); 
        }
    }
}
