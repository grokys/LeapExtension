using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class EditCommandModel : CommandModel
    {
        public EditCommandModel()
            : base("edit")
        {
        }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("document_id")]
        public string DocumentId { get; set; }
    }
}
