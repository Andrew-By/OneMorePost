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
using MimeKit;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OneMorePost.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        private DateTime _lastMessageDate;
        private List<Models.EmailMessage> _parsedMessages;

        public MailController()
        {
            _lastMessageDate = new DateTime();
            _parsedMessages = new List<Models.EmailMessage>();
        }

        public List<Models.EmailMessage> GetParsedMessages()
        {
            return _parsedMessages;
        }

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

                    client.Authenticate("XXX", "YYY");

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    // >>> DEBUG
                    sb.AppendLine(String.Format("Using time: {0}", _lastMessageDate.ToString()));
                    sb.AppendLine(String.Format("Total messages: {0}", inbox.Count));
                    sb.AppendLine(String.Format("Recent messages: {0}", inbox.Recent));
                    // <<< DEBUG

                    _parsedMessages.Clear();
                    var attachments = new List<MimePart>();
                    var multiparts = new List<Multipart>();
                    var uids = inbox.Search(SearchQuery.DeliveredAfter(_lastMessageDate));
                    MimeMessage message = new MimeMessage();

                    // >>> TODO: pass Account to get associated messages
                    List<Models.Account> accs = new List<Models.Account>();
                    Models.Account acc1 = new Models.Account(),
                        acc2 = new Models.Account();
                    acc1.Email = new Models.EmailAccount();
                    acc2.Email = new Models.EmailAccount();
                    acc1.Email.Email = "no-reply@accounts.google.com";
                    acc2.Email.Email = "XXX"; // enter email to filter its messages
                    accs.Add(acc1);
                    accs.Add(acc2);
                    // <<< TODO

                    foreach (var uid in uids)
                    {
                        message = inbox.GetMessage(uid);
                        if (accs.Any(x => (x.Email.Email == message.From.Mailboxes.First().Address))) // ugly filtering
                        {
                            var parsedMessage = new Models.EmailMessage();
                            parsedMessage.Author = new Models.Author();
                            parsedMessage.Attachments = new List<Tuple<string, Stream>>();
                            parsedMessage.Id = message.MessageId;
                            parsedMessage.Author.Name = message.From.Mailboxes.First().Name;
                            parsedMessage.Author.Email = message.From.Mailboxes.First().Address;
                            parsedMessage.Subject = message.Subject;
                            parsedMessage.Body = message.TextBody;
                            parsedMessage.ReceivedDate = message.Date.DateTime;

                            using (var iter = new MimeIterator(message))
                            {
                                // collect our list of attachments and their parent multiparts
                                while (iter.MoveNext())
                                {
                                    var multipart = iter.Parent as Multipart;
                                    var part = iter.Current as MimePart;

                                    if (multipart != null && part != null && part.IsAttachment)
                                    {
                                        // keep track of each attachment's parent multipart
                                        multiparts.Add(multipart);
                                        attachments.Add(part);
                                    }
                                }
                            }

                            // now remove each attachment from its parent multipart...
                            for (int i = 0; i < attachments.Count; i++)
                                multiparts[i].Remove(attachments[i]);

                            // >>> DEBUG
                            if (attachments.Count > 0)
                                sb.AppendLine(String.Format("Total attachments: {0}", attachments.Count));
                            // <<< DEBUG

                            foreach (var attachment in attachments)
                            {
                                var fileName = attachment.FileName;

                                using (var stream = new MemoryStream())
                                {
                                    attachment.ContentObject.DecodeTo(stream);

                                    // >>> DEBUG
                                    stream.Position = 0;
                                    sb.AppendLine(String.Format("Attachment \"{0}\":", fileName));
                                    using (var reader = new StreamReader(stream, Encoding.ASCII))
                                    {
                                        string line;
                                        while ((line = reader.ReadLine()) != null)
                                        {
                                            sb.AppendLine(line);
                                        }
                                    }
                                    // <<< DEBUG

                                    parsedMessage.Attachments.Add(new Tuple<string, System.IO.Stream>(fileName, stream));
                                }
                            }

                            _parsedMessages.Add(parsedMessage);
                        }
                    }

                    _lastMessageDate = message.Date.DateTime;

                    // >>> DEBUG
                    sb.AppendLine(String.Format("Last message time: {0}", _lastMessageDate.ToString()));
                    sb.AppendLine(String.Format("Filtered messages count: {0}", _parsedMessages.Count));
                    // <<< DEBUG
                }
                catch (AuthenticationException ex)
                {
                    // >>> DEBUG
                    sb.AppendLine(ex.Message);
                    sb.AppendLine(ex.ToString());
                    // <<< DEBUG
                }
                catch (MailKit.Net.Imap.ImapProtocolException ex)
                {
                    // >>> DEBUG
                    sb.AppendLine("Replace XXX and YYY with actual mail and password, dude!");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine(ex.ToString());
                    // <<< DEBUG
                }
                finally
                {
                    client.Disconnect(true);
                }
            }

            return sb.ToString();
        }

        // POST api/mail
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
    }
}
