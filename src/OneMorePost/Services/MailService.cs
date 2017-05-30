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
using OneMorePost.Data;
using Microsoft.EntityFrameworkCore;
using MailKit;

namespace OneMorePost.Services
{
    public class MailService : Interfaces.IMailService
    {
        private readonly OneMoreContext _context;

        public MailService(OneMoreContext context)
        {
            _context = context;
        }

        public IList<EmailMessage> GetNewMessages(int accountId)
        {
            var account = _context.Accounts.Include(a => a.EmailAccount).FirstOrDefault(a => a.Id == accountId);
            var newMessages = new List<EmailMessage>();

            if (account != null)
            {
                EmailAccount eAcc = account.EmailAccount;

                using (var client = new ImapClient())
                {
                    var credentials = new NetworkCredential(eAcc.UserName, eAcc.Password);

                    client.Connect(eAcc.ServerUri, eAcc.ServerPort, eAcc.ServerUseSSL);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(credentials);

                    client.Inbox.Open(FolderAccess.ReadOnly);

                    // Обрабатываем сами письма

                    foreach (var summary in client.Inbox.Fetch((int)eAcc.LastMessageUid, -1, MessageSummaryItems.UniqueId))
                    {
                        if (summary.UniqueId.Id > (int)eAcc.LastMessageUid)
                        {
                            MimeMessage iMessage = client.Inbox.GetMessage(summary.UniqueId);

                            // Если в белом списке есть запись "*", то принимаем письма от всех отправителей
                            if (eAcc.WhileListFrom.Contains("*") || eAcc.WhileListFrom.Contains(iMessage.From.Mailboxes.First().Address))
                            {

                                // Текстовое содержимое

                                var message = new EmailMessage
                                {
                                    Id = (int)summary.UniqueId.Id,
                                    From = iMessage.From.Mailboxes.First().Name,
                                    Subject = iMessage.Subject,
                                    Body = iMessage.TextBody,
                                    ReceivedDate = iMessage.Date.LocalDateTime
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
                                            Contents = ms.ToArray()
                                        });
                                    }
                                }

                                newMessages.Add(message);
                            }

                            eAcc.LastMessageUid = (int)summary.UniqueId.Id; // Учитываем Id сообщения, даже если оно не подошло
                        }
                    }
                }

                _context.SaveChanges();
            }

            return newMessages;
        }
    }
}
