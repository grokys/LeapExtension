using System;
using Newtonsoft.Json;

namespace LeapExtension.Models
{
    class CommandModel
    {
        public CommandModel(string command)
        {
            Command = command;
        }

        [JsonProperty("command")]
        public string Command { get; }
    }
}
