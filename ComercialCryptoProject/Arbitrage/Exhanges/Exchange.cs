using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Binance.Net.Clients;
using BingX.Net.Clients;
using Bitget.Net.Clients;
using Bybit.Net.Clients;
using CoinEx.Net.Clients;
using GateIo.Net.Clients;
using Kucoin.Net.Clients;
using Mexc.Net.Clients;
using OKX.Net.Clients;

namespace CryptoPtoject.Arbitrage.Exchanges
{
    /// <summary>
    /// абстрактный класс, предназначенный для работы с биржами.
    /// </summary>
    abstract class Exchange
    {
        public decimal bidsPrice { get; set; } = 0;
        public decimal avgBuyPrice { get; set; } = 0;
        public decimal asksPrice { get; set; } = 0;
        public decimal avgSellPrice { get; set; } = 0;
        public decimal bidQuantity { get; set; } = 0;
        public decimal askQuantity { get; set; } = 0;
        public decimal takerFeeRate { get; set; }
        public decimal makerFeeRate { get; set; }
        public string[] tokenList { get;set; }
        public List<decimal> blockChainWithdrawFee { get; set; }
        public List<string> blockChainFullName { get; set; }
        public List<string> blockChain { get; set; }
        public List<bool> blockChainIsWithdrawable { get; set; }
        public List<bool> blockChainIsDepozitable { get; set; }
        public abstract void GetOrderBook(string pareName);
        public abstract void GetFeeRate(string pareName);
        public abstract void GetTradePares();
        public Exchange()
        {
            avgSellPrice = 0;
            avgBuyPrice = 0;
            bidsPrice = 0;
            asksPrice = 0;
            bidQuantity = 0;
            askQuantity = 0;
        }
    }
}
