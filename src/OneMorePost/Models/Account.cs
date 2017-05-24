using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    public class Account
    {
        public int Id { get; set; }
        public EmailAccount EmailAccount { get; set; }
        public VKAccount VKAccount { get; set; }
        public List<TelegramAccount> TelegramAccounts { get; set; }
    }
}
