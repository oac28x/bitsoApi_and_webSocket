using System;
using WebSocket.Enums;

namespace WebSocket.Interfaces
{
    public interface IAPIWebSocket
    {
        event EventHandler<string> OnTradeMessage;

        void Init(params Coin[] coinsSuscription);

        void Stop();
        void Restart();
    }
}
