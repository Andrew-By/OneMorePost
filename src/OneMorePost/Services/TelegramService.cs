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
        enum EState
        {
            InSubscibe,
            InUnsubscribe
        }

        private readonly IOptions<TelegramSettings> settings;
        private readonly TelegramBotClient botClient;
        private Dictionary<long, EState> accountsState = new Dictionary<long, EState>();

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
            if (accountsState.ContainsKey(fromUser.Id))
            {
                switch (accountsState[fromUser.Id])
                {
                    case EState.InSubscibe:
                        if (doSubscribe(fromUser, message))
                        {
                            botClient.SendTextMessageAsync(fromUser.Id, "Вы подписаны");
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(fromUser.Id,
                                "Не удалось подписаться, нет такого идентификатора");
                        }
                        break;
                    case EState.InUnsubscribe:
                        if (doUnsubscibe(fromUser, message))
                        {
                            botClient.SendTextMessageAsync(fromUser.Id, "Вы отписаны");
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(fromUser.Id,
                                "Не удалось отписаться, нет такой записи в подписках");
                        }
                        break;
                }
                accountsState.Remove(fromUser.Id);
            }
            else
            {
                Start(fromUser);
            }
        }

        public void Start(TelegramAccount user)
        {
            botClient.SendTextMessageAsync(user.Id, "Вы можете вызвать /help для получения возможных команд");
        }

        public void Help(TelegramAccount user)
        {
            botClient.SendTextMessageAsync(user.Id, "Команды:\n" +
                "/help - Вывести возможные команды\n" +
                "/subscribe - Подписаться на новые письма\n" +
                "/unsubscribe - Отписаться от получения писем\n");
        }

        public void Subscribe(TelegramAccount follower)
        {
            if (accountsState.ContainsKey(follower.Id))
            {
                accountsState.Remove(follower.Id);
            }
            accountsState.Add(follower.Id, EState.InSubscibe);
            botClient.SendTextMessageAsync(follower.Id, "Пожалуйста, введите уникальный идентификатор подписки");
        }

        public void Unsubscribe(TelegramAccount follower)
        {
            if (accountsState.ContainsKey(follower.Id))
            {
                accountsState.Remove(follower.Id);
            }
            accountsState.Add(follower.Id, EState.InUnsubscribe);
            botClient.SendTextMessageAsync(follower.Id, "Пожалуйста, введите уникальный идентификатор подписки");
        }

        private bool doSubscribe(TelegramAccount follower, string guid)
        {
            // TODO: Implement
            return false;
        }

        private bool doUnsubscibe(TelegramAccount follower, string guid)
        {
            // TODO: Implement
            return false;
        }
    }
}
