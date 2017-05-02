using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Imap;
using MailKit.Security;
using MailKit;
using MailKit.Search;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OneMorePost.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        // GET api/mail
        [HttpGet]
        public string Get()
        {
            StringBuilder sb = new StringBuilder();

            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                try
                {
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate("user", "pass");

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    sb.AppendLine(String.Format("Total messages: {0}", inbox.Count));
                    sb.AppendLine(String.Format("Recent messages: {0}", inbox.Recent));

                    /*var uids = inbox.Search(SearchQuery.All);

                    foreach (var uid in uids)
                    {
                        // https://github.com/jstedfast/MailKit
                        var message = inbox.GetMessage(uid);

                        // write the message to a file
                        //message.WriteTo(string.Format("{0}.eml", uid));
                        sb.AppendLine(String.Format("Subject: {0}", message.ToString()));
                    }*/

                    foreach (var summary in inbox.Fetch(0, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId))
                    {
                        sb.AppendLine(String.Format("[summary] {0:D2}: {1}", summary.Index, summary.Envelope.Subject));
                    }
                    // TODO: parse messages and attachments
                }
                catch (AuthenticationException ex)
                {
                    sb.AppendLine(ex.Message);
                    sb.AppendLine(ex.ToString());
                }
                finally
                {
                    client.Disconnect(true);
                }
            }

            return sb.ToString();
        }

        // GET api/mail/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return id.ToString();
        }

        // POST api/mail
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/mail/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/mail/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
