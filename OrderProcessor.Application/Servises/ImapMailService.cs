
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using MimeKit;
using OrderProcessor.Application.Interfaces;
using OrderProcessor.Domain.Entities;
using OrderProcessor.Domain.Interfaces;


namespace OrderProcessor.Application.Services
{
    public class ImapMailService : IImapMailService
    {
        private readonly IEmailEntityRepository _emailEntityRepository;
        private readonly IConfiguration _configuration;

        public ImapMailService(IEmailEntityRepository emailEntityRepository, IConfiguration configuration)
        {
            _emailEntityRepository = emailEntityRepository;
            _configuration = configuration;
        }

        public async Task LoadEmailsAsync()
        {
            var emlPath1 = Path.Combine(AppContext.BaseDirectory, "Mails", "Zamowienie1.eml");
            var emlPath2 = Path.Combine(AppContext.BaseDirectory, "Mails", "Zamowienie2.eml");

            var message1 = MimeMessage.Load(emlPath1);
            var message2 = MimeMessage.Load(emlPath2);

            using var client = new SmtpClient();
            client.Connect("greenmail", 3025, MailKit.Security.SecureSocketOptions.None);
            client.Send(message1);
            client.Send(message2);
            client.Disconnect(true);
        }

        public async Task StoreEmailsAsync()
        {
            var host = _configuration["Imap:Server"];
            var port = int.Parse(_configuration["Imap:Port"]);
            var user = _configuration["Imap:Username"];
            var pass = _configuration["Imap:Password"];

            using var client = new ImapClient();
            await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.None);
            await client.AuthenticateAsync(user, pass);
            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);

            var uids = await inbox.SearchAsync(SearchQuery.NotSeen);
            foreach (var uid in uids)
            {
                var message = await inbox.GetMessageAsync(uid);

                AddEmail(message);

                await _emailEntityRepository.SaveChangesAsync();
            }

            await client.DisconnectAsync(true);
        }

        private void AddEmail(MimeMessage message)
        {
            var email = new EmailEntity
            {
                From = message.From.ToString(),
                Subject = message.Subject,
                Date = message.Date.UtcDateTime,
                BodyText = message.TextBody ?? "",
                BodyHtml = message.HtmlBody ?? "",
                RawEml = SaveEmlToBytes(message),
                Attachments = message.Attachments.Select(SaveAttachmentToEntity).ToList()
            };

            _emailEntityRepository.AddAsync(email);
        }

        private byte[] SaveEmlToBytes(MimeMessage message)
        {
            using var stream = new MemoryStream();
            message.WriteTo(stream);
            return stream.ToArray();
        }

        private AttachmentEntity SaveAttachmentToEntity(MimeEntity attachment)
        {
            if (attachment is MimePart part)
            {
                using var stream = new MemoryStream();
                part.Content.DecodeTo(stream);

                return new AttachmentEntity
                {
                    FileName = part.FileName,
                    ContentType = part.ContentType.MimeType,
                    Data = stream.ToArray()
                };
            }
            return null;
        }
    }
}
