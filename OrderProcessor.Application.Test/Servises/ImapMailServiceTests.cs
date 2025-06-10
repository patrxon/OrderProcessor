using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Moq;
using OrderProcessor.Application.Services;
using OrderProcessor.Domain.Entities;
using OrderProcessor.Domain.Interfaces;

namespace OrderProcessor.Application.Tests.Services
{
    public class ImapMailServiceTests
    {
        private readonly Mock<IEmailEntityRepository> _emailRepoMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly ImapMailService _service;

        public ImapMailServiceTests()
        {
            _emailRepoMock = new Mock<IEmailEntityRepository>();
            _configMock = new Mock<IConfiguration>();
            _service = new ImapMailService(_emailRepoMock.Object, _configMock.Object);
        }

        [Fact]
        public void SaveEmlToBytes_ReturnsByteArray()
        {
            // Arrange
            var message = new MimeMessage();
            message.Subject = "Test";
            var method = typeof(ImapMailService).GetMethod("SaveEmlToBytes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = (byte[])method.Invoke(_service, new object[] { message });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public void SaveAttachmentToEntity_ReturnsAttachmentEntity()
        {
            // Arrange
            var content = new byte[] { 1, 2, 3 };
            var part = new MimePart("application", "octet-stream")
            {
                FileName = "test.txt",
                Content = new MimeContent(new MemoryStream(content))
            };
            var method = typeof(ImapMailService).GetMethod("SaveAttachmentToEntity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = (AttachmentEntity)method.Invoke(_service, new object[] { part });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test.txt", result.FileName);
            Assert.Equal("application/octet-stream", result.ContentType);
            Assert.Equal(content, result.Data);
        }
    }
}
