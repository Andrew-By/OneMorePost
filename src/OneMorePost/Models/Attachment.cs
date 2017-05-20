using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    public class Attachment
    {
        public string Title { get; set; }
        public Stream Contents { get; set; }
    }
}
