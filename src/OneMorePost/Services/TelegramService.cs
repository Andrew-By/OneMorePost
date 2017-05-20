using OneMorePost.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneMorePost.Models;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OneMorePost.Services
{
    public class TelegramService : ITelegramService
    {
        enum EState
        {
            InSubscibe,
            InUnsubscribe
        }

        private readonly IOptions<TelegramSettings> _settings;
        private readonly TelegramBotClient _botClient;
        private readonly Dictionary<ChatId, EState> _accountsState = new Dictionary<ChatId, EState>();

        public TelegramService(IOptions<TelegramSettings> settings)
        {
            _settings = settings;
            _botClient = new TelegramBotClient(settings.Value.BotId);
        }

        public void MakePost(TelegramAccount toUser, string message)
        {
            _botClient.SendTextMessageAsync(toUser.Id, message);
        }

        public void OnMessage(TelegramAccount fromUser, string message)
        {
            if (_accountsState.ContainsKey(fromUser.Id))
            {
                switch (_accountsState[fromUser.Id])
                {
                    case EState.InSubscibe:
                        if (doSubscribe(fromUser, message))
                        {
                            _botClient.SendTextMessageAsync(fromUser.Id, "Вы подписаны");
                        }
                        else
                        {
                            _botClient.SendTextMessageAsync(fromUser.Id,
                                "Не удалось подписаться, нет такого идентификатора");
                        }
                        break;
                    case EState.InUnsubscribe:
                        if (doUnsubscibe(fromUser, message))
                        {
                            _botClient.SendTextMessageAsync(fromUser.Id, "Вы отписаны");
                        }
                        else
                        {
                            _botClient.SendTextMessageAsync(fromUser.Id,
                                "Не удалось отписаться, нет такой записи в подписках");
                        }
                        break;
                }
                _accountsState.Remove(fromUser.Id);
            }
            else
            {
                Start(fromUser);
            }
        }

        public void Start(TelegramAccount user)
        {
            _botClient.SendTextMessageAsync(user.Id, "Вы можете вызвать /help для получения возможных команд");
        }

        public void Help(TelegramAccount user)
        {
            _botClient.SendTextMessageAsync(user.Id, "Команды:\n" +
                "/help - Вывести возможные команды\n" +
                "/subscribe - Подписаться на новые письма\n" +
                "/unsubscribe - Отписаться от получения писем\n");
        }

        public void Subscribe(TelegramAccount follower)
        {
            if (_accountsState.ContainsKey(follower.Id))
            {
                _accountsState.Remove(follower.Id);
            }
            _accountsState.Add(follower.Id, EState.InSubscibe);
            _botClient.SendTextMessageAsync(follower.Id, "Пожалуйста, введите уникальный идентификатор подписки");
        }

        public void Unsubscribe(TelegramAccount follower)
        {
            if (_accountsState.ContainsKey(follower.Id))
            {
                _accountsState.Remove(follower.Id);
            }
            _accountsState.Add(follower.Id, EState.InUnsubscribe);
            _botClient.SendTextMessageAsync(follower.Id, "Пожалуйста, введите уникальный идентификатор подписки");
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
