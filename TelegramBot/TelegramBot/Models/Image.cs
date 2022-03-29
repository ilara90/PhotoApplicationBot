using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramBot.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public byte[]? ImageByte { get; set; }
        public List<Tag> Tags { get; set; } = new();
        public List<ImagesTags> ImagesTags { get; set; } = new();
    }
}
