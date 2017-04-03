using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class UpdateCommandModel : CommandModel
    {
        public UpdateCommandModel()
            : base("update")
        {
        }

        [JsonProperty("position")]
        public int Position { get; set; }
    }
}
