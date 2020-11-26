using System;
using WebSocket.Interfaces;

namespace WebSocket.Controlers
{
    public class BitsoTradingControler : IBitsoTrading
    {
        public BitsoTradingControler(IAPIPublic _bPublic, IAPIPrivate _bPrivate, ITelegramReporter _tr)
        {
        }
    }
}
