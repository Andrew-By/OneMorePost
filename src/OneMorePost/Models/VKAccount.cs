using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    public class VKAccount
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string AccessToken { get; set; }
    }
}
