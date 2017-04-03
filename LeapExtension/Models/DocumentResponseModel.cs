using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class DocumentResponseModel : ResponseModel
    {
        [JsonProperty("leap_document")]
        public LeapDocumentModel LeapDocument { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
}
