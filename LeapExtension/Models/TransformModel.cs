using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class TransformModel
    {
        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("num_delete")]
        public int NumDelete { get; set; }

        [JsonProperty("insert")]
        public string Insert { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
}
