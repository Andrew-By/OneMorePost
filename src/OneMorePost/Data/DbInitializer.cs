using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Data
{
    public static class DbInitializer
    {
        public static void Initialize(OneMoreContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
