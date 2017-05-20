using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneMorePost.Interfaces;
using OneMorePost.Models;

namespace OneMorePost.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IMailService _mail;

        public ValuesController(IMailService mail)
        {
            _mail = mail;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<EmailMessage> Get()
        {
            var account = new Account()
            {
                Email = new EmailAccount()
                {
                    Email = "onemoreposttest@gmail.com",
                    ServerUri = "imap.gmail.com",
                    ServerPort = 993,
                    ServerUseSSL = true,
                    UserName = "onemoreposttest",
                    Password = "d0300oi8fa815{B",
                }
            };

            return _mail.GetNewMessages(account);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
