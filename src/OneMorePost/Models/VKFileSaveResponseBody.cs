using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    public class VKFileSaveResponseBody
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("ext")]
        public string Ext { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
