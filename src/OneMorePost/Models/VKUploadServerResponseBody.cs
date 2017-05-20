using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    public class VKUploadServerResponseBody
    {
        [JsonProperty("upload_url")]
        public string UploadUrl { get; set; }
    }
}
