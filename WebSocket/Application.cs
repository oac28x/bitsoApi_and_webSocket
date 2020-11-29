using System;
using WebSocket.Interfaces;

namespace WebSocket
{
    public class Application
    {
        protected readonly IBitsoNotifier bitsoNotifier;
        protected readonly IAPIWebSocket bitsoWebSocket;
        protected readonly IBitsoService bitsoService;
        protected readonly IBitsoTrading bitsoTrading;
        protected readonly IPrivateTelegramReporter ptr;

        public Application(IBitsoService _bitsoService,
            IBitsoNotifier _bitsoNotifier,
            IAPIWebSocket _bitsoWebSocket,
            IBitsoTrading _bitsoTrading,
            IPrivateTelegramReporter _ptr)
        {
            bitsoWebSocket = _bitsoWebSocket;
            bitsoNotifier = _bitsoNotifier;
            bitsoService = _bitsoService;
            bitsoTrading = _bitsoTrading;
            ptr = _ptr;
        }


        public void Start()
        {
            //Both need same coins, it is possible to pass using array or one by one
            bitsoWebSocket.Init(ConfigData.books);  
            bitsoNotifier.Init(ConfigData.books);

            //Bitso Trading API
            bitsoService.SetEnvironment(ConfigData.IsProduction);
            bitsoTrading.Init();
        }

        public void Stop()
        {
            //
            //tps?.SendMessageTest("Servicio Detenido");
        }

        public void Restart()
        {
            //
            //tps?.SendMessageTest("Servicio Reiniciado");
        }
    }
}
