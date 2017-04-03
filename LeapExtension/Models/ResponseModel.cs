using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class ResponseModel
    {
        [JsonProperty("response_type")]
        public string ResponseType { get; set; }
    }
}
