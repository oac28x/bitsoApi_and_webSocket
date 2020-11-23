using System;
namespace WebSocket.Enums
{
    //Coins available in Bitso, and websocket coins subscription
    public enum Coin
    {
        btc, eth, ltc, xrp, gnt, bat, bch, dai, mana
    }

    //Log service priorities
    public enum LogPriority
    {
        high,
        medium,
        low
    }
}
