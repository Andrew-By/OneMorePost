using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    public class VKFileSaveResponse
    {
        [JsonProperty("response")]
        public List<VKFileSaveResponseBody> Response { get; set; }
    }
}
