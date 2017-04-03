using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class UserUpdateModel
    {
        [JsonProperty("client")]
        public ClientModel Client { get; set; }

        [JsonProperty("message")]
        public UpdateMessageModel Message { get; set; }
    }
}
