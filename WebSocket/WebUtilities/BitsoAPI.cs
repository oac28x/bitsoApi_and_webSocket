using System;
using WebSocket.Interfaces;

namespace WebSocket.WebUtilities
{
    public class BitsoAPI
    {
        protected IBitsoService BitsoClient { get; private set; }

        protected BitsoAPI(IBitsoService bistoClient)
        {
            BitsoClient = bistoClient;
        }
    }
}
