using Application.KonsiCredit.UserBenefitsViewModels;
using Microsoft.Extensions.Configuration;
using Moq;
using RabbitMQ.Client;
using Services.KonsiCredit.BenefitsAppService;
using Services.KonsiCredit.Factory;
using Xunit;

namespace ProducerQueueAppService.Test;

public class ProducerQueueAppServiceTest
{
    public class ProducerQueueAppServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private readonly Mock<IBenefitsAppService> _benefitsAppServiceMock = new Mock<IBenefitsAppService>();
        private readonly Mock<IModel> _channelMock = new Mock<IModel>();
        private readonly Mock<IRabbitMqChannelFactory> _channelFactory = new Mock<IRabbitMqChannelFactory>();
        private readonly List<UserBenefitsViewModel> _testUsers = new List<UserBenefitsViewModel>
        {
            new UserBenefitsViewModel { data = new Data("00022200011", Array.Empty<Benefits>())},
            new UserBenefitsViewModel { data = new Data("00022200012", Array.Empty<Benefits>())}
        };

        [Fact]
        public async Task EnqueueCpf_ValidCpf_EnqueuesToQueue()
        {
            // Arrange
            _configurationMock.Setup(x => x.GetSection("CpfQueue").Value).Returns("CpfQueueTest");
            
            _channelFactory.Setup(x => x.CreateChannel()).Returns(_channelMock.Object);

            var producerQueueAppService = new Services.KonsiCredit.QueueAppService.ProducerQueueAppService(
                _configurationMock.Object,
                _benefitsAppServiceMock.Object,
                _channelFactory.Object
            );

            _benefitsAppServiceMock
                .Setup(service => service.GetUserBenefits(It.IsAny<string>()))
                .ReturnsAsync(new UserBenefitsViewModel { data = new Data("00022200013", Array.Empty<Benefits>())});

            // Act
            await producerQueueAppService.EnqueueCpf();

            // Assert
            _channelMock.Verify(
                channel => channel.BasicPublish(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<IBasicProperties>(),
                    It.IsAny<ReadOnlyMemory<byte>>()
                ),
                Times.Exactly(8)
            );
        }

        // Add more test cases as needed
    }
}
