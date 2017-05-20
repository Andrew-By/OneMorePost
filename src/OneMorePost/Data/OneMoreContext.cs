using Microsoft.EntityFrameworkCore;
using OneMorePost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Data
{
    public class OneMoreContext : DbContext
    {
        public OneMoreContext(DbContextOptions<OneMoreContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
    }
}
