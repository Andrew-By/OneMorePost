using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    public class VKUploadResponseBody
    {
        [JsonProperty("file")]
        public string File { get; set; }
    }
}
