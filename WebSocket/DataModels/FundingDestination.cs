using System;
using Newtonsoft.Json;

namespace WebSocket.DataModels
{
    public class FundingDestination
    {
        [JsonProperty("account_identifier_name")]
        public string AccountIdentifierName { get; set; }

        [JsonProperty("account_identifier")]
        public string AccountIdentifier { get; set; }
    }
}
