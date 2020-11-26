using System;

namespace WebSocket.Interfaces
{
    public interface IAPIWebSocket
    {
        event EventHandler<string> OnTradeMessage;

        void Init(params string[] coinsSuscription);

        void Stop();
        void Restart();
    }
}
