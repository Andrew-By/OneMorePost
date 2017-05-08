using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OneMorePost.Interfaces;
using OneMorePost.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneMorePost.Services
{
    public class VKService : IVKService
    {
        private const string VKApi = "https://api.vk.com/method";
        private const string VKOAuth = "https://oauth.vk.com";
        private const string VKVersion = "5.63";
        private const string RedirectUri = "http://onemorepost.azurewebsites.net/api/VK/Auth";

        private readonly VKOptions options;

        public VKService(IOptions<VKOptions> optionsAccessor)
        {
            options = optionsAccessor.Value;
        }

        public void MakePost(int userId, string message)
        {
            throw new NotImplementedException();
        }
    }
}
