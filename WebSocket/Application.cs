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
            bitsoWebSocket.Init(ConfigData.books);  
            bitsoNotifier.Init(ConfigData.books);


            //Bitso Trading API
            //bitsoService.SetEnvironment(ConfigData.IsProduction);
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
