namespace TelegramBot.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<Image> Images { get; set; } = new();
        public List<ImagesTags> ImagesTags { get; set; } = new();
    }
}
