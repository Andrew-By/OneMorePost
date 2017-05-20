using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    /// <summary>
    /// Учётная запись электронной почты
    /// </summary>
    public class EmailAccount
    {
        private const char SEPARATOR = ';';

        public EmailAccount()
        {
            LastMessageDate = new DateTime();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServerUri { get; set; }
        public int ServerPort { get; set; }
        public bool ServerUseSSL { get; set; }
        public DateTime LastMessageDate { get; set; }
        public string InternalWhileListFrom { get; set; }
        [NotMapped]
        public List<string> WhileListFrom
        {
            get
            {
                var list = new List<string>();
                if(!string.IsNullOrEmpty(InternalWhileListFrom))
                {
                    string[] addr = InternalWhileListFrom.Split(SEPARATOR);
                    list.AddRange(addr);
                }
                return list;
            }
            set
            {
                InternalWhileListFrom = string.Join(SEPARATOR.ToString(), value);
            }
        }
    }
}
