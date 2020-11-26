using System;
using Newtonsoft.Json;

namespace WebSocket.DataModels
{
    public class BankCode
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
