using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            Attachments = new List<Attachment>();
        }

        public int Id { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime ReceivedDate { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
