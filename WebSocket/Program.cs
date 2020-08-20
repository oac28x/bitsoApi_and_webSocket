using System;
using WebSocket.WebUtilities;

namespace WebSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create instance WebSokets to listen live trades of suscribed currencies.
            APIWebSoketChannels webSockets = new APIWebSoketChannels();
            webSockets.Init();

            //Connect to API and test request balance.
            APIWebClient api = new APIWebClient();
            api.RunTest();

            Console.ReadLine();
        }
        
    }
}
