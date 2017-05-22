﻿using OneMorePost.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneMorePost.Models;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using OneMorePost.Data;
using Microsoft.EntityFrameworkCore;

namespace OneMorePost.Services
{
    public class TelegramService : ITelegramService
    {
        enum EState
        {
            InSubscibe,
            InUnsubscribe
        }

        private readonly OneMoreContext _context;
        private readonly IOptions<TelegramSettings> _settings;
        private readonly TelegramBotClient _botClient;
        private readonly Dictionary<long, EState> _accountsState = new Dictionary<long, EState>();

        public TelegramService(IOptions<TelegramSettings> settings, OneMoreContext context)
        {
            _context = context;
            _settings = settings;
            _botClient = new TelegramBotClient(settings.Value.BotId);
        }

        public async Task MakePostAsync(int accountId, string message, IList<string> attachments)
        {
            var account = _context.Accounts.Include(a => a.TelegramAccounts).FirstOrDefault(a => a.Id == accountId);
            if (account != null && account.TelegramAccounts != null)
            {
                foreach (var telegram in account.TelegramAccounts)
                {
                    await _botClient.SendTextMessageAsync(telegram.Id, message);
                }
            }
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
                                "Не удалось подписаться, нет такого идентификатора или вы уже подписаны на этот аккаунт");
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
            int accountId;
            if (Int32.TryParse(guid, out accountId))
            {
                var account = _context.Accounts.Include(a => a.TelegramAccounts).FirstOrDefault(a => a.Id == accountId);
                if (account != null && !account.TelegramAccounts.Contains(follower))
                {
                    account.TelegramAccounts.Add(follower);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        private bool doUnsubscibe(TelegramAccount follower, string guid)
        {
            int accountId;
            if (Int32.TryParse(guid, out accountId))
            {
                var account = _context.Accounts.Include(a => a.TelegramAccounts).FirstOrDefault(a => a.Id == accountId);
                if (account != null && account.TelegramAccounts.Contains(follower))
                {
                    bool result = account.TelegramAccounts.Remove(follower);
                    _context.SaveChanges();
                    return result;
                }
            }
            return false;
        }
    }
}
