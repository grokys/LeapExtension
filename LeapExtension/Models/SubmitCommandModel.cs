using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class SubmitCommandModel : CommandModel
    {
        public SubmitCommandModel()
            : base("submit")
        {
        }

        [JsonProperty("transform")]
        public TransformModel Transform { get; set; }
    }
}
