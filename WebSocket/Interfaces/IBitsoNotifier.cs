using System;
using WebSocket.DataModels;

namespace WebSocket.Interfaces
{
    public interface IBitsoNotifier
    {
        event EventHandler<CoinDataModel> OnTradeUp;
        event EventHandler<CoinDataModel> OnTradeDown;
        event EventHandler<CoinDataModel> OnLotTradeUp;

        void Init(params string[] coinsSuscription);
    }
}
