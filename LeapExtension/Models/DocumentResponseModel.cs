using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class DocumentResponseModel
    {
        [JsonProperty("response_type")]
        public string ResponseType { get; set; }

        [JsonProperty("leap_document")]
        public LeapDocumentModel LeapDocument { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
}
