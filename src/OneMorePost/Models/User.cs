using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    /// <summary>
    /// Пользователь сервиса. Тот, чью почту мы проверяем и в чью группу постим
    /// </summary>
    public class User
    {
        public int Id { get; set; } // Совпадает с VK Id пользователя
        public EmailAccount EmailAccount { get; set; }
        public int GroupId { get; set; } // Ид. группы, в которую происходит пост

    }
}
