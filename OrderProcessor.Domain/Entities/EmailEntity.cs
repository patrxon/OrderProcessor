
namespace OrderProcessor.Domain.Entities
{
    public class EmailEntity
    {
        public int Id { get; set; }
        public string? From { get; set; }
        public string? Subject { get; set; }
        public DateTime Date { get; set; }
        public string? BodyText { get; set; }
        public string? BodyHtml { get; set; }
        public byte[]? RawEml { get; set; }

        public List<AttachmentEntity>? Attachments { get; set; }
    }
}
