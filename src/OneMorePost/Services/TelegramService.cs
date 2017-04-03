using OneMorePost.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneMorePost.Models;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace OneMorePost.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly IOptions<TelegramSettings> settings;
        private readonly TelegramBotClient botClient;

        public TelegramService(IOptions<TelegramSettings> settings)
        {
            this.settings = settings;
            botClient = new TelegramBotClient(settings.Value.BotId);
            // TODO: Think about better way to set webhook, and unset webhook on the end of working
            // TODO: It's not working for now (I think because we have no ssl certificate).
            //       see: https://core.telegram.org/bots/faq#i-39m-having-problems-with-webhooks

            botClient.SetWebhookAsync("https://matrohin-onemorepost.azurewebsites.net/api/telegram").Wait();
        }

        public void PostInfo(TelegramAccount toUser, string message)
        {
            botClient.SendTextMessageAsync(toUser.Id, message);
        }
    }
}
