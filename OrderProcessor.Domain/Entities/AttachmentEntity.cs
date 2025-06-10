
namespace OrderProcessor.Domain.Entities
{
    public class AttachmentEntity
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }

        public int EmailEntityId { get; set; }
    }
}
