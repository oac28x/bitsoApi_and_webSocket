using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WebSocket.Utilities
{
    /// <summary>
    /// Relative Strength Index (RSI)
    /// </summary>
    public class RSI
    {
        private List<double> OhlcList { get; set; }
        private int Period { get; set; }

        public RSI() { }

        /// <summary>
        ///    RS = Average Gain / Average Loss
        ///    
        ///                  100
        ///    RSI = 100 - --------
        ///                 1 + RS
        /// </summary>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:relative_strength_index_rsi"/>
        /// <returns></returns>
        public RSISerie Calculate(int period, List<decimal> history)
        {
            RSISerie rsiSerie = new RSISerie();
            if (history.Count < period) return rsiSerie;

            Period = period;
            OhlcList = history.Select(x => (double)x).ToList();


            double gainSum = 0;
            double lossSum = 0;
            for (int i = 1; i < Period; i++)
            {
                double thisChange = OhlcList[i] - OhlcList[i - 1];
                if (thisChange > 0)
                {
                    gainSum += thisChange;
                }
                else
                {
                    lossSum += (-1) * thisChange;
                }
            }

            var averageGain = gainSum / Period;
            var averageLoss = lossSum / Period;
            var rs = averageGain / averageLoss;
            rsiSerie.RS.Add(Math.Round(rs, 2, MidpointRounding.AwayFromZero));
            var rsi = 100 - (100 / (1 + rs));
            rsiSerie.RSI.Add(Math.Round(rsi, 2, MidpointRounding.AwayFromZero));

            for (int i = Period + 1; i < OhlcList.Count; i++)
            {
                double thisChange = OhlcList[i] - OhlcList[i - 1];
                if (thisChange > 0)
                {
                    averageGain = (averageGain * (Period - 1) + thisChange) / Period;
                    averageLoss = (averageLoss * (Period - 1)) / Period;
                }
                else
                {
                    averageGain = (averageGain * (Period - 1)) / Period;
                    averageLoss = (averageLoss * (Period - 1) + (-1) * thisChange) / Period;
                }
                rs = averageGain / averageLoss;
                rsiSerie.RS.Add(Math.Round(rs, 2, MidpointRounding.AwayFromZero));
                rsi = 100 - (100 / (1 + rs));
                rsiSerie.RSI.Add(Math.Round(rsi, 2, MidpointRounding.AwayFromZero));
            }

            return rsiSerie;
        }
    }

    public class RSISerie
    {
        public List<double?> RSI { get; set; }
        public List<double?> RS { get; set; }

        public RSISerie()
        {
            RSI = new List<double?>();
            RS = new List<double?>();
        }
    }
}
