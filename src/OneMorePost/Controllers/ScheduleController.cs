using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneMorePost.Data;
using OneMorePost.Interfaces;
using OneMorePost.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OneMorePost.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        private readonly OneMoreContext _context;
        private readonly IMailService _mailService;
        private readonly IVKService _vkService;

        public ScheduleController(OneMoreContext context, IMailService mailService, IVKService vkService)
        {
            _context = context;
            _mailService = mailService;
            _vkService = vkService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            foreach(var account in _context.Accounts.Include(a=>a.VKAccount).Include(a=>a.EmailAccount))
            {
                if(account.VKAccount!=null)
                {
                    if(account.EmailAccount!=null)
                    {
                        List<EmailMessage> messages = _mailService.GetNewMessages(account.Id).ToList();
                        foreach (var message in messages)
                            _vkService.MakePostAsync(account.Id, message.Body);
                    }
                }
            }
            return Ok();
        }
    }
}
