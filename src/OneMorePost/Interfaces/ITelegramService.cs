using OneMorePost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Interfaces
{
    public interface ITelegramService
    {
        void PostInfo(TelegramAccount toUser, string message);
    }
}
