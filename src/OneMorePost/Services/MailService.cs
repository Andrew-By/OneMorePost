using OneMorePost.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneMorePost.Models;
using MailKit.Net.Imap;
using System.Net;
using MailKit.Search;
using MimeKit;
using System.IO;

namespace OneMorePost.Services
{
    public class MailService : IMailService
    {
        public IList<EmailMessage> GetNewMessages(Account account)
        {
            EmailAccount eAcc = account.Email;
            var newMessages = new List<EmailMessage>();

            using (var client = new ImapClient())
            {
                var credentials = new NetworkCredential(eAcc.UserName, eAcc.Password);

                client.Connect(eAcc.ServerUri, eAcc.ServerPort, eAcc.ServerUseSSL);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(credentials);

                client.Inbox.Open(MailKit.FolderAccess.ReadOnly);

                // Создаём фильтр

                var query = SearchQuery.DeliveredAfter(eAcc.LastMessageDate);
                
                if (!eAcc.WhileListFrom.Contains("*")) // Если в белом списке есть запись "*", то принимаем письма от всех отправителей
                {
                    var fromQuery = new SearchQuery();
                    foreach (var addr in eAcc.WhileListFrom)
                        fromQuery.Or(SearchQuery.FromContains(addr));
                    query.And(fromQuery);
                }

                // Обрабатываем сами письма

                foreach (var uid in client.Inbox.Search(query))
                {
                    MimeMessage iMessage = client.Inbox.GetMessage(uid);

                    // Текстовое содержимое

                    var message = new EmailMessage
                    {
                        Id = iMessage.MessageId,
                        From = iMessage.From.Mailboxes.First().Name,
                        Subject = iMessage.Subject,
                        Body = iMessage.TextBody,
                        ReceivedDate = iMessage.Date.DateTime
                    };

                    // Вложения

                    var multiparts = new List<Multipart>();
                    var attachments = new List<MimePart>();
                    using (var iter = new MimeIterator(iMessage))
                    {
                        while (iter.MoveNext())
                        {
                            var multipart = iter.Parent as Multipart;
                            var part = iter.Current as MimePart;

                            if (multipart != null && part != null && part.IsAttachment)
                            {
                                multiparts.Add(multipart);
                                attachments.Add(part);
                            }
                        }
                    }

                    for (int i = 0; i < attachments.Count; i++)
                        multiparts[i].Remove(attachments[i]);

                    foreach (var attachment in attachments)
                    {
                        using (var ms = new MemoryStream())
                        {
                            attachment.ContentObject.DecodeTo(ms);

                            message.Attachments.Add(new Attachment
                            {
                                Title = attachment.FileName,
                                Contents = ms
                            });
                        }
                    }

                    newMessages.Add(message);

                    eAcc.LastMessageDate = message.ReceivedDate;
                }
            }

            return newMessages;
        }
    }
}
