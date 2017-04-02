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
        public int Id { get; set; }
        public string Email { get; set; }

        // TODO: Здесь стоит добавить параметры подключения к почтовому серверу
    }
}
