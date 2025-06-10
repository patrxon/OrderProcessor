using Moq;
using OrderProcessor.Application.Interfaces;
using OrderProcessor.Application.Services;
using OrderProcessor.Domain.DTOs;
using OrderProcessor.Domain.Entities;
using OrderProcessor.Domain.Interfaces;

namespace OrderProcessor.Application.Tests.Services
{
    public class EmailProcessingServiceTests
    {
        private readonly Mock<IEmailEntityRepository> _emailRepoMock;
        private readonly Mock<ILanguageModelService> _languageModelMock;
        private readonly EmailProcessingService _service;

        public EmailProcessingServiceTests()
        {
            _emailRepoMock = new Mock<IEmailEntityRepository>();
            _languageModelMock = new Mock<ILanguageModelService>();
            _service = new EmailProcessingService(_emailRepoMock.Object, _languageModelMock.Object);
        }

        [Fact]
        public async Task ProcessEmails_ReturnsAllOrderInformation()
        {
            // Arrange
            var emails = new List<EmailEntity>
            {
                new EmailEntity { Id = 1, BodyHtml = "<b>Order1</b>" },
                new EmailEntity { Id = 2, BodyHtml = "<i>Order2</i>" }
            };
            var order1 = new List<OrderInformation> { new OrderInformation { ProductName = "A", Quantity = 1, Price = 10 } };
            var order2 = new List<OrderInformation> { new OrderInformation { ProductName = "B", Quantity = 2, Price = 20 } };

            _emailRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(emails);
            _languageModelMock.Setup(l => l.ExtractOrderInfoAsync(It.IsAny<string>()))
                .ReturnsAsync((string emailText) =>
                {
                    if (emailText.Contains("Order1")) return order1;
                    if (emailText.Contains("Order2")) return order2;
                    return new List<OrderInformation>();
                });

            // Act
            var result = await _service.ProcessEmails();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, o => o.ProductName == "A");
            Assert.Contains(result, o => o.ProductName == "B");
        }

        [Theory]
        [InlineData("<b>Test</b>", "_Test_")]
        [InlineData("<p>Hello<br>World</p>", "_Hello_World_")]
        [InlineData("NoHtml", "NoHtml")]
        [InlineData("<div>\nTest\t</div>", "_Test_")]
        public void StripHtml_RemovesHtmlAndWhitespace(string input, string expected)
        {
            var method = typeof(EmailProcessingService)
                .GetMethod("StripHtml", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var result = (string)method.Invoke(_service, new object[] { input });

            Assert.Equal(expected, result);
        }
    }
}
