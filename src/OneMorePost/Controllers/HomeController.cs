using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneMorePost.Data;
using OneMorePost.Models;

namespace WebApplication1.Controllers
{

    public class HomeController : Controller
    {
        private readonly OneMoreContext _context;

        public HomeController(OneMoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(
            int userId, int groupId, string authToken,
            string email, string server, int port, bool ssl, string login, string password)
        {
            _context.Add(new Account
            {
                VKAccount = new VKAccount()
                {
                    Id = userId,
                    GroupId = groupId,
                    AccessToken = authToken
                },
                EmailAccount = new EmailAccount
                {
                    Email = email,
                    ServerUri = server,
                    ServerPort = port,
                    ServerUseSSL = ssl,
                    UserName = login,
                    Password = password,
                    InternalWhileListFrom = "*"
                }
            });
            _context.SaveChanges();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
