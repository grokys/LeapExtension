using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class UpdateResponseModel : ResponseModel
    {
        [JsonProperty("user_updates")]
        public UserUpdateModel[] UserUpdates { get; set; }
    }
}
