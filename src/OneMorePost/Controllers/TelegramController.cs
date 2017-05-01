using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneMorePost.Interfaces;
using OneMorePost.Models;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

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
            service.PostInfo(new TelegramAccount { Id=message.Message.From.Id }, message.Message.Text);
        }
    }
}
