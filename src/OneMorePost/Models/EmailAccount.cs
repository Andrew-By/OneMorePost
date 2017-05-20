using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    /// <summary>
    /// Учётная запись электронной почты
    /// </summary>
    public class EmailAccount
    {
        public EmailAccount()
        {
            LastMessageDate = new DateTime();
            WhileListFrom = new List<string>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServerUri { get; set; }
        public int ServerPort { get; set; }
        public bool ServerUseSSL { get; set; }
        public DateTime LastMessageDate { get; set; }
        public List<string> WhileListFrom { get; set; }
    }
}
