namespace TelegramBot.Models
{
    public class ImagesTags
    {
        public int ImageId { get; set; }
        public Image? Image { get; set; }

        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
