using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class ClientModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }
    }
}
