using System;
using Newtonsoft.Json;

namespace WebSocket.DataModels
{
    public class BitsoTradeDataModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("book")]
        public string Book { get; set; }
        [JsonProperty("payload")]
        public BookPayload[] Payload { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("response")]
        public string SubscripStatus { get; set; }  //Ok value response
    }

    public class BookPayload
    {
        [JsonProperty("v")]
        public decimal AmountMXN { get; set; }
        [JsonProperty("r")]
        public decimal PriceMXN { get; set; }
        [JsonProperty("a")]
        public decimal AmountCoin { get; set; }
        [JsonProperty("t")]
        public bool Type { get; set; }
    }
}
