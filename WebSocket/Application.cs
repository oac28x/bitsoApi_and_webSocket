using System;
using WebSocket.Interfaces;

namespace WebSocket
{
    public class Application
    {
        protected readonly IPrivateTelegramReporter tps;
        protected readonly IBitsoNotifier bitsoNotifier;
        protected readonly IAPIWebSocket bitsoWebSocket;
        protected readonly IBitsoService bitsoService;
        protected readonly IAPIPrivate bitsoPrivate;


        private bool IsProduction = true;
        //Bitso available coins to pass on Init(...)
        private readonly string[] books = {
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

        public Application(IBitsoService _bitsoService, IAPIPrivate _bitsoPrivate, IBitsoNotifier _bitsoNotifier, IAPIWebSocket _bitsoWebSocket, IPrivateTelegramReporter _telegramPrivateService)
        {
            bitsoService = _bitsoService;
            bitsoPrivate = _bitsoPrivate;

            bitsoNotifier = _bitsoNotifier;
            bitsoWebSocket = _bitsoWebSocket;
            tps = _telegramPrivateService;
        }


        public void Start()
        {
            //Both need same coins, it is possible to pass using array or one by one
            bitsoWebSocket.Init(books);  
            bitsoNotifier.Init(books);


            //Bitso Trading API
            //bitsoService.SetEnvironment(IsProduction);
            //var test = bitsoPrivate.GetAccountStatus();

            //Console.WriteLine(test.FirstName);
            //Console.WriteLine(test.LastName);
            //Console.WriteLine(test.ClientId);
        }

        public void Stop()
        {
            //
            tps?.SendMessageTest("Servicio Detenido");
        }

        public void Restart()
        {
            //
            tps?.SendMessageTest("Servicio Reiniciado");
        }
    }
}
