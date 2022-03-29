using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoApplication.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public byte[]? ImageByte { get; set; }
        public List<Tag> Tags { get; set; } = new();       
        [NotMapped]
        public List<int>? TagIds { get; set; }
        public List<ImagesTags> ImagesTags { get; set; } = new();
    }
}
