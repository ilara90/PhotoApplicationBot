using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBot.Models;
using TelegramBot.Services;

namespace TelegramBot.Controllers
{     
    public class TelegramBotController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
                                          [FromBody] Update update)
        {
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }

    }
}
