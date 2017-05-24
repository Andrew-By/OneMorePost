using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneMorePost.Interfaces;
using OneMorePost.Models;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OneMorePost.Controllers
{
    [Route("api/[controller]")]
    public class TelegramController : Controller
    {
        private readonly ITelegramService _service;

        public TelegramController(ITelegramService service)
        {
            _service = service;
        }

        // POST api/telegram
        [HttpPost]
        public void Post([FromBody] Update message)
        {
            if (message.Type == UpdateType.MessageUpdate)
            {
                var from = new TelegramAccount { ChatId = message.Message.From.Id };
                switch (message.Message.Text)
                {
                    case "/start":
                        _service.Start(from);
                        break;
                    case "/help":
                        _service.Help(from);
                        break;
                    case "/subscribe":
                        _service.Subscribe(from);
                        break;
                    case "/unsubscribe":
                        _service.Unsubscribe(from);
                        break;
                    default:
                        _service.OnMessage(from, message.Message.Text);
                        break;
                }
            }
        }
    }
}
