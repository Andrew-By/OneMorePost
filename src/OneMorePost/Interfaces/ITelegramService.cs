using OneMorePost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Interfaces
{
    public interface ITelegramService
    {
        Task MakePostAsync(int accountId, string message, IList<string> attachments);
        void OnMessage(TelegramAccount fromUser, string message);
        void Start(TelegramAccount user);
        void Help(TelegramAccount user);
        void Subscribe(TelegramAccount follower);
        void Unsubscribe(TelegramAccount follower);
    }
}
