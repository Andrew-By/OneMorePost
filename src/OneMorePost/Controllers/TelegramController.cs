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
        private readonly ITelegramService service;

        public TelegramController(ITelegramService service)
        {
            this.service = service;
        }

        // POST api/telegram
        [HttpPost]
        public void Post([FromBody] Update message)
        {
            if (message.Type == UpdateType.MessageUpdate)
            {
                var from = new TelegramAccount { Id = message.Message.From.Id };
                switch (message.Message.Text)
                {
                    case "/subscribe":
                        service.Subscribe(from);
                        break;
                    case "/unsubscribe":
                        service.Unsubscribe(from);
                        break;
                    default:
                        service.OnMessage(from, message.Message.Text);
                        break;
                }
            }
        }
    }
}
