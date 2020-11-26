using System;
using Newtonsoft.Json;

namespace WebSocket.DataModels
{
    public class MobilePhoneNumber
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
    }
}
