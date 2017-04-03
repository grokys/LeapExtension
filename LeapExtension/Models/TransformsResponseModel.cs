using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class TransformsResponseModel : ResponseModel
    {
        [JsonProperty("transforms")]
        public TransformModel[] Transforms { get; set; }
    }
}
