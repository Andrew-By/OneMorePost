﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMorePost.Models
{
    public class VKUploadServerResponse
    {
        [JsonProperty("response")]
        public VKUploadServerResponseBody Response { get; set; }
    }
}
