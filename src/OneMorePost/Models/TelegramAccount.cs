using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace OneMorePost.Models
{
    public class TelegramAccount : IEquatable<TelegramAccount>
    {
        public int Id { get; set; }
        public long ChatId { get; set; }

        public bool Equals(TelegramAccount other)
        {
            return ChatId == other.ChatId;
        }
    }
}
