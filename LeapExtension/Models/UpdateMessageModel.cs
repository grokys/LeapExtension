using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class UpdateMessageModel
    {
        [JsonProperty("position")]
        public int? Position { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
