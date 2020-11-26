using System;
namespace WebSocket
{
    public static class ConfigData
    {
        public const bool IsProduction = true;

        public const string BITSO_KEY = "";
        public const string BITSO_SECRET = "";

        public const string TelegramApiToken = "";
        public const string TelegramPublicChatId = "";
        public const string TelegramMyChatId = "";

        //Bitso available coins to pass on Init(...)
        public static readonly string[] books =  {
            "btc_mxn",
            "eth_mxn",
            "ltc_mxn",
            "xrp_mxn",
            "gnt_mxn",
            "bat_mxn",
            "bch_mxn",
            "dai_mxn",
            "mana_mxn"
        };
    }
}
