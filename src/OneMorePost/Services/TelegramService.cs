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
        }

        public void MakePost(TelegramAccount toUser, string message)
        {
            botClient.SendTextMessageAsync(toUser.Id, message);
        }

        public void OnMessage(TelegramAccount fromUser, string message)
        {
            botClient.SendTextMessageAsync(fromUser.Id, message);
        }

        public void Subscribe(TelegramAccount follower)
        {
            // TODO: Implement
            botClient.SendTextMessageAsync(follower.Id, "Not implemented so far");
        }

        public void Unsubscribe(TelegramAccount follower)
        {
            // TODO: Implement
            botClient.SendTextMessageAsync(follower.Id, "Not implemented so far");
        }


    }
}
