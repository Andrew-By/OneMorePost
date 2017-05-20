using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OneMorePost.Data;
using OneMorePost.Interfaces;
using OneMorePost.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneMorePost.Services
{
    public class VKService : IVKService
    {
        private const string VKApi = "https://api.vk.com/method";
        private const string VKOAuth = "https://oauth.vk.com";
        private const string RedirectUri = "http://onemorepost.azurewebsites.net/api/VK/Auth";

        private readonly OneMoreContext _context;
        private readonly VKOptions _options;

        public VKService(IOptions<VKOptions> optionsAccessor, OneMoreContext context)
        {
            _options = optionsAccessor.Value;
            _context = context;
        }

        public async Task MakePostAsync(int accountId, string message, IList<string> attachments)
        {
            var account = _context.Accounts.Include(a => a.VKAccount).FirstOrDefault(a => a.Id == accountId);
            if (account != null && account.VKAccount != null)
            {
                var client = new HttpClient();

                // https://vk.com/dev/wall.post
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("owner_id", '-' + account.VKAccount.GroupId.ToString()),
                    new KeyValuePair<string, string>("friends_only", "0"),
                    new KeyValuePair<string, string>("from_group", "1"),
                    new KeyValuePair<string, string>("message", message),
                    new KeyValuePair<string, string>("attachments", string.Join(",", attachments)),
                    new KeyValuePair<string, string>("signed", "0"),

                    new KeyValuePair<string, string>("access_token", account.VKAccount.AccessToken),
                    new KeyValuePair<string, string>("v", _options.Version)
                });

                await client.PostAsync(VKApi + "/wall.post", content);
            }
        }

        public async Task<string> UploadFileAsync(int accountId, Attachment file)
        {
            var account = _context.Accounts.Include(a => a.VKAccount).FirstOrDefault(a => a.Id == accountId);
            if (account != null && account.VKAccount != null)
            {
                using (var client = new HttpClient())
                {

                    // Получаем адрес сервера для отправки файла
                    // https://vk.com/dev/docs.getWallUploadServer
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("group_id", account.VKAccount.GroupId.ToString()),

                        new KeyValuePair<string, string>("access_token", account.VKAccount.AccessToken),
                        new KeyValuePair<string, string>("v", _options.Version)
                    });

                    HttpResponseMessage response = await client.PostAsync(VKApi + "/docs.getWallUploadServer", content);
                    string result = await response.Content.ReadAsStringAsync();

                    VKUploadServerResponse vkUploadServerResponse = JsonConvert.DeserializeObject<VKUploadServerResponse>(result);

                    // Отправляем файл
                    if (vkUploadServerResponse.Response != null)
                    {
                        using (var data = new MultipartFormDataContent())
                        {
                            data.Add(new StreamContent(new MemoryStream(file.Contents)), "file", file.Title);
                            response = await client.PostAsync(vkUploadServerResponse.Response.UploadUrl, data);
                            result = await response.Content.ReadAsStringAsync();

                            VKUploadResponseBody fileUploadResponse = JsonConvert.DeserializeObject<VKUploadResponseBody>(result);
                            
                            if(fileUploadResponse!=null)
                            {
                                // Сохраняем файл
                                content = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("file", fileUploadResponse.File),

                                    new KeyValuePair<string, string>("access_token", account.VKAccount.AccessToken),
                                    new KeyValuePair<string, string>("v", _options.Version)
                                });

                                response = await client.PostAsync(VKApi + "/docs.save", content);
                                result = await response.Content.ReadAsStringAsync();

                                VKFileSaveResponse vkFileSaveResponse = JsonConvert.DeserializeObject<VKFileSaveResponse>(result);

                                if (vkFileSaveResponse.Response != null)
                                    return $"doc{vkFileSaveResponse.Response[0].OwnerId}_{vkFileSaveResponse.Response[0].Id}";
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
