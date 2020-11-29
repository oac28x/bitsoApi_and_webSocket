using System;
using System.Collections.Generic;
using System.Linq;
using WebSocket.DataModels;
using WebSocket.Interfaces;
using WebSocket.WebUtilities;

namespace WebSocket.Controlers
{
    public class BitsoTradingControler : IBitsoTrading, IDisposable
    {
        private protected IAPIPrivate bitsoPrivate;
        private protected IAPIPublic bitsoPublic;
        private protected IBitsoNotifier bitsoNotifier;
        private protected IPrivateTelegramReporter tr;

        IBitsoService bs;

        public BitsoTradingControler(IBitsoNotifier _bn, IAPIPublic _bPublic, IAPIPrivate _bPrivate, IPrivateTelegramReporter _tr, IBitsoService _bs)
        {
            bitsoPrivate = _bPrivate;
            bitsoPublic = _bPublic;
            bitsoNotifier = _bn;
            tr = _tr;

            bs = _bs;
        }

        public void Init()
        {
            bitsoNotifier.OnTradeUp += BitsoNotifier_OnTradeUp;
            bitsoNotifier.OnTradeDown += BitsoNotifier_OnTradeDown;
            bitsoNotifier.OnLotTradeUp += BitsoNotifier_OnLotTradeUp;

            //Select mxn books


            List<BookInfo> booksInfo = bitsoPublic.GetAvailableBooks().Where(x => ConfigData.books.Contains(x.Book)).ToList();
            Dictionary<string, decimal> minAmount = booksInfo.ToDictionary(key => key.Book, book => book.MinimumAmountAsDecimal, StringComparer.OrdinalIgnoreCase);



            FeeInfo feeInfo = bitsoPrivate.GetFees();
            Dictionary<string, decimal> dictionary = feeInfo.Fees.Where(x => ConfigData.books.Contains(x.Book)).ToDictionary(key => key.Book, fee => fee.FeeMakerDecimalASDecimal, StringComparer.OrdinalIgnoreCase);


        }

        private void BitsoNotifier_OnLotTradeUp(object sender, CoinDataModel e)
        {
            //Console.WriteLine("Notificando Lot Trade UP: " + e.CoinName);
        }

        private void BitsoNotifier_OnTradeUp(object sender, CoinDataModel e)
        {
            //Console.WriteLine("Notificando Trade UP: " + e.CoinName);
        }

        private void BitsoNotifier_OnTradeDown(object sender, CoinDataModel e)
        {
            //Console.WriteLine("Notificando Trade DOWN: " + e.CoinName);
        }

        public void Dispose()
        {
            bitsoNotifier.OnTradeUp -= BitsoNotifier_OnTradeUp;
            bitsoNotifier.OnTradeDown -= BitsoNotifier_OnTradeDown;
            bitsoNotifier.OnLotTradeUp -= BitsoNotifier_OnLotTradeUp;

            GC.SuppressFinalize(this);
        }
    }
}
