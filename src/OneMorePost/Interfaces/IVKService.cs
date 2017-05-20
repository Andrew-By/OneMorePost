using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Interfaces
{
    public interface IVKService
    {
        Task MakePostAsync(int accountId, string message);
    }
}
