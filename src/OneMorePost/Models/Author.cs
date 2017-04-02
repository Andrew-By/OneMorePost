﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    /// <summary>
    /// Автор письма. Для белого списка.
    /// </summary>
    public class Author
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public User User { get; set; }
    }
}
