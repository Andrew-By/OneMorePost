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

        // TODO: Delete after testing
        private List<string> history = new List<string>();

        public TelegramController(ITelegramService service)
        {
            this.service = service;
        }

        // GET api/telegram
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return history;
        }

        // POST api/telegram
        [HttpPost]
        public void Post(Update message)
        {
            history.Add(message.Message.Text);
            service.PostInfo(new TelegramAccount { Id=message.Id }, message.Message.Text);
        }
    }
}
