using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public class HandleUpdateService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleUpdateService> _logger;
        private readonly ApplicationContext db;

        public HandleUpdateService(ITelegramBotClient botClient, ILogger<HandleUpdateService> logger, ApplicationContext context)
        {
            _botClient = botClient;
            _logger = logger;
            db = context;
        }
        public async Task EchoAsync(Update update)
        {
            if (update == null) return;
            var message = update.Message;
            try
            {
                if (message?.Type == MessageType.Text)
                {
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Hello," + message.From.FirstName);
                }

                if (message?.Type == MessageType.Document)
                {
                    await SaveInDataBase(message, message.Document.FileName, message.Document.FileId);

                    await _botClient.SendTextMessageAsync(message.Chat.Id, "I'll keep the image!");
                }

                if (message?.Type == MessageType.Photo)
                {
                    var fileName = "photo" + message.Photo[message.Photo.Count() - 1].FileUniqueId + ".jpg";
                    var fileId = message.Photo[message.Photo.Count() - 1].FileId;
                    await SaveInDataBase(message, fileName, fileId);

                    await _botClient.SendTextMessageAsync(message.Chat.Id, "I'll keep the photo!");
                }
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task SaveInDataBase(Message message, string fileName, string fileId)
        {
            Image image = new Image();
            Tag tag = new Tag();
            image.Title = fileName;
            byte[] fileByte;
            var file = await _botClient.GetFileAsync(fileId);

            using (MemoryStream ms = new MemoryStream())
            {
                await _botClient.DownloadFileAsync(file.FilePath, ms);
                fileByte = ms.ToArray();
            }

            image.ImageByte = fileByte;
            List<string> tagsWord = new List<string>();
            IQueryable<Tag> tags = db.Tags;
            if (message.Caption != null)
            {
                foreach (var entity in message.CaptionEntities.Where(x => x.Type == MessageEntityType.Hashtag))
                {
                    tagsWord.Add(message.Caption.Substring(entity.Offset, entity.Length));
                }
            }
            else
            {
                tagsWord.Add(DateTime.Now.ToString("#ddMMyyyy"));
            }

            var existingTagsTitle = tags.Where(x => tagsWord.Contains(x.Title)).Select(x => x.Title).ToList();
            var existingTags = tags.Where(x => tagsWord.Contains(x.Title));
            image.Tags.AddRange(existingTags);
            tagsWord = tagsWord.Except(existingTagsTitle).ToList();
            foreach (var tagWord in tagsWord)
            {
                tag.Title = tagWord;
                db.Tags.AddRange(tag);
                image.Tags.Add(tag);
            }

            db.Images.Add(image);
            await db.SaveChangesAsync();
        }
    }
}
