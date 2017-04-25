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
        private const string VKEndPointUri = "https://oauth.vk.com";
        private const string RedirectUri = "http://onemorepost.azurewebsites.net/api/VK/Auth";

        private readonly VKOptions options;

        public VKService(IOptions<VKOptions> optionsAccessor)
        {
            options = optionsAccessor.Value;
        }

        public string GetAccessToken(string code)
        {
            var client = new HttpClient();
            string result = client.GetStringAsync($"{VKEndPointUri}/access_token?client_id={options.AppId}&client_secret={options.AppSecret}&redirect_uri={RedirectUri}&code={code}").Result;
            AccessTokenResponse response = JsonConvert.DeserializeObject<AccessTokenResponse>(result);
            return response.access_token;
        }

        public void MakePost(int userId, string message)
        {
            throw new NotImplementedException();
        }
    }
}
