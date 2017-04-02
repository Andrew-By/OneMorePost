using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Interfaces
{
    public interface IVKService
    {
        int GetAccessToken(int code);
        void MakePost(int userId, string message);
    }
}
