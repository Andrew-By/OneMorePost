﻿using OneMorePost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Interfaces
{
    public interface IMailService
    {
        IList<EmailMessage> GetNewMessages(int accountId);
    }
}
