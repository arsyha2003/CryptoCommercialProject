using BingX.Net.Clients;
using Bitget.Net.Clients;
using Bybit.Net.Clients;
using CoinEx.Net.Clients;
using GateIo.Net.Clients;
using Kucoin.Net.Clients;
using OKX.Net.Clients;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiBot
{   
    /// <summary>
    /// класс занимающийся проверкой арбитражных ситуаций по паре
    /// </summary>
    public class GoArbitrage
    {
        private TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
        private int buyIndex;
        private int sellIndex;

        private decimal fullFee;
        private decimal withdrawalFee; 
        private decimal spread;
        private decimal profit;
        private decimal avgBuyQuantity;
        private decimal avgSellQuantity;
        private decimal avgBuyPrice;
        private decimal avgSellPrice;   
        private decimal lastBuyPrice;
        private decimal lastSellPrice;
        private decimal takerFeeRate;
        private decimal makerFeeRate;
        private decimal activeQuantity;
        private decimal usdtQuantity;

        private string buyingExchange;
        private string sellingExchange;
        private string sellLink;
        private string buyLink;
        private decimal[] withdrawalFees;

        private List<string> firstExchangeChains = new List<string>();
        private List<string> secondExchangeChains = new List<string>();
        private List<string> chainsFullName1 = new List<string>();
        private List<string> chainsFullName2 = new List<string>();
        private List<bool> isWithdraw = new List<bool>();
        private List<bool> isDepozit = new List<bool>();

        private BybitExchange bybit;
        private BinanceExchange binance;
        private OKXExchange okx;
        private BitGetExchange bitget;
        private MexcExchange mexc;
        private CoinExExchange coinex;
        private GateIoExchange gateIo;
        private BingXExchange bingX;
        private KucoinExchange kucoin;

        public decimal spreadRange1=(decimal)0.2;
        public decimal spreadRange2 = 6;
        private string currentPare;
        public GoArbitrage()
        {
            bybit = new BybitExchange();
            binance = new BinanceExchange();
            okx = new OKXExchange();
            bitget = new BitGetExchange();
            mexc = new MexcExchange();
            coinex = new CoinExExchange();
            gateIo = new GateIoExchange();
            bingX = new BingXExchange();
            kucoin = new KucoinExchange();
        }
        public GoArbitrage(decimal spreadRange1, decimal spreadRange2)
        {
            bybit = new BybitExchange();
            binance = new BinanceExchange();
            okx = new OKXExchange();
            bitget = new BitGetExchange();
            mexc = new MexcExchange();
            coinex = new CoinExExchange();
            gateIo = new GateIoExchange();
            bingX = new BingXExchange();
            kucoin = new KucoinExchange();
            this.spreadRange1 = spreadRange1;
            this.spreadRange2 = spreadRange2;
        }
        private void NullAllData()
        {
            buyLink = string.Empty;
            sellLink = string.Empty;
            buyingExchange = string.Empty;
            sellingExchange = string.Empty;
            activeQuantity = 0;
            makerFeeRate = 0;
            takerFeeRate = 0;
            lastBuyPrice = 0;
            lastSellPrice = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            avgBuyQuantity = 0;
            avgSellQuantity = 0;
            profit = 0;
            spread = 0;
            withdrawalFee = 0;
            fullFee = 0;
        }
        private void GetFullData(string pare)
        {
            currentPare = pare;
            NullAllData();
            Task[] tasks = new Task[]
            {
                Task.Run(() => bybit.GetOrderBook(pare)),
                Task.Run(() => binance.GetOrderBook(pare)),
                Task.Run(() => mexc.GetOrderBook(pare)),
                Task.Run(() => bitget.GetOrderBook(pare)),
                Task.Run(() => gateIo.GetOrderBook(pare)),
                Task.Run(() => okx.GetOrderBook(pare)),
                Task.Run(() => coinex.GetOrderBook(pare)),
                Task.Run(() => bingX.GetOrderBook(pare)),
                Task.Run(() => kucoin.GetOrderBook(pare)),

                Task.Run(() => bybit.GetFeeRate(pare)),
                Task.Run(() => binance.GetFeeRate(pare)),
                Task.Run(() => mexc.GetFeeRate(pare)),
                Task.Run(() => bitget.GetFeeRate(pare)),
                Task.Run(() => gateIo.GetFeeRate(pare)),
                Task.Run(() => okx.GetFeeRate(pare)),
                Task.Run(() => coinex.GetFeeRate(pare)),
                Task.Run(() => bingX.GetFeeRate(pare)),
                Task.Run(() => kucoin.GetFeeRate(pare))
            };
            Task.WaitAll(tasks);
        }
        /// <summary>
        /// метод на получение всех торговых пар со всех бирж
        /// </summary>
        public async Task<string[]> GetTradeParesAsync()
        {
            var tokens = new List<string>();
            Random randomShuffleNumber = new Random();
            Task[] tasks = new Task[]
            {
                Task.Run(()=>bybit.GetTradePares()),
                Task.Run(()=>okx.GetTradePares()),
                Task.Run(()=>bitget.GetTradePares()),
                Task.Run(()=>mexc.GetTradePares()),
                Task.Run(()=>coinex.GetTradePares()),
                Task.Run(()=>gateIo.GetTradePares()),
                Task.Run(()=>bingX.GetTradePares()),
                Task.Run(()=>kucoin.GetTradePares())
            };
            Task.WaitAll(tasks);
            try
            {
                tokens.AddRange(bybit.tokenList);
                tokens.AddRange(okx.tokenList);
                tokens.AddRange(bitget.tokenList);
                tokens.AddRange(mexc.tokenList);
                tokens.AddRange(coinex.tokenList);
                tokens.AddRange(gateIo.tokenList);
                tokens.AddRange(bingX.tokenList);
                tokens.AddRange(kucoin.tokenList);
            }
            catch { }
            List<string> shuffledTokensList = tokens.OrderBy(a => randomShuffleNumber.Next()).Distinct().ToList();
            return shuffledTokensList.ToArray();
        }
        private int GetBuyIndex(decimal[] buyPrices)
        {
            try
            {
                decimal buyPrice = 0;
                for (int i = 0; i < buyPrices.Length; i++)
                {
                    if (buyPrices[i] != 0)
                    {
                        buyPrice = buyPrices[i];
                        break;
                    }
                }
                for (int i = Array.IndexOf(buyPrices, buyPrice); i < buyPrices.Length; i++)
                {
                    if (buyPrice > buyPrices[i] && buyPrices[i] != 0) buyPrice = buyPrices[i];
                }
                if (buyPrice == 0) return -1;
                else return Array.IndexOf(buyPrices, buyPrice);
            }
            catch { return -1; }
        }
        private int GetSellIndex(decimal[] sellPrices)
        {
            try
            {
                decimal sellPrice = 0;
                for (int i = 0; i < sellPrices.Length; i++)
                {
                    if (sellPrices[i] != 0)
                    {
                        sellPrice = sellPrices[i];
                        break;
                    }
                }
                for (int i = Array.IndexOf(sellPrices, sellPrice); i < sellPrices.Length; i++)
                {
                    if (sellPrice < sellPrices[i] && sellPrices[i] != 0) sellPrice = sellPrices[i];
                }
                if (sellPrice == 0) return -1;
                else return Array.IndexOf(sellPrices, sellPrice);
            }
            catch { return -1; }
        }
        private decimal GetWithdrawalData(decimal[] withdrawalFees)
        {
            try
            {
                decimal withdrawFee = 0;
                for (int i = 0; i < withdrawalFees.Length; i++)
                {
                    if (withdrawalFees[i] != 0)
                    {
                        withdrawFee = withdrawalFees[i];
                        break;
                    }
                }
                for (int i = 0; i < withdrawalFees.Length; i++)
                {
                    if (withdrawFee > withdrawalFees[i] && withdrawalFees[i] != 0)
                    {
                        withdrawFee = withdrawalFees[i];
                    }
                }
                return withdrawFee;
            }
            catch { return -1; }
        }
        private State SelectData()
        {
            string[] names = new string[] { "Bybit", "Binance", "Mexc", "Bitget", "GateIo", "OKX", "CoinEx", "BingX", "Kucoin" };
            decimal[] asks = new decimal[] { bybit.asksPrice, binance.asksPrice, mexc.asksPrice, bitget.asksPrice, gateIo.asksPrice, okx.asksPrice, coinex.asksPrice, bingX.asksPrice, kucoin.asksPrice };
            decimal[] bids = new decimal[] { bybit.bidsPrice, binance.bidsPrice, mexc.bidsPrice, bitget.bidsPrice, gateIo.bidsPrice, okx.bidsPrice, coinex.bidsPrice, bingX.bidsPrice, kucoin.bidsPrice };
            decimal[] avgBuyPrices = new decimal[] { bybit.avgBuyPrice, binance.avgBuyPrice, mexc.avgBuyPrice, bitget.avgBuyPrice, gateIo.avgBuyPrice, okx.avgBuyPrice, coinex.avgBuyPrice, bingX.avgBuyPrice, kucoin.avgBuyPrice };
            decimal[] avgSellPrices = new decimal[] { bybit.avgSellPrice, binance.avgSellPrice, mexc.avgSellPrice, bitget.avgSellPrice, gateIo.avgSellPrice, okx.avgSellPrice, coinex.avgSellPrice, bingX.avgSellPrice, kucoin.avgSellPrice };
            decimal[] bidQuantities = new decimal[] { bybit.bidQuantity, binance.bidQuantity, mexc.bidQuantity, bitget.bidQuantity, gateIo.bidQuantity, okx.bidQuantity, coinex.bidQuantity, bingX.bidQuantity, kucoin.bidQuantity };
            decimal[] askQuantities = new decimal[] { bybit.askQuantity, binance.askQuantity, mexc.askQuantity, bitget.askQuantity, gateIo.askQuantity, okx.askQuantity, coinex.askQuantity, bingX.askQuantity, kucoin.askQuantity };
            decimal[] tFees = new decimal[] { bybit.takerFeeRate, binance.takerFeeRate, mexc.takerFeeRate, bitget.takerFeeRate, gateIo.takerFeeRate, okx.takerFeeRate, coinex.takerFeeRate, bingX.takerFeeRate, kucoin.takerFeeRate };
            decimal[] mFees = new decimal[] { bybit.makerFeeRate, binance.makerFeeRate, mexc.makerFeeRate, bitget.makerFeeRate, gateIo.makerFeeRate, okx.makerFeeRate, coinex.makerFeeRate, bingX.makerFeeRate, kucoin.makerFeeRate };
            List<decimal>[] fees = new List<decimal>[] { bybit.blockChainWithdrawFee, binance.blockChainWithdrawFee, mexc.blockChainWithdrawFee, bitget.blockChainWithdrawFee, gateIo.blockChainWithdrawFee, okx.blockChainWithdrawFee, coinex.blockChainWithdrawFee, bingX.blockChainWithdrawFee, kucoin.blockChainWithdrawFee };
            List<string>[] chains = new List<string>[] { bybit.blockChain, binance.blockChain, mexc.blockChain, bitget.blockChain, gateIo.blockChain, okx.blockChain, coinex.blockChain, bingX.blockChain, kucoin.blockChain };
            List<bool>[] isWithdrawable = new List<bool>[] { bybit.blockChainIsWithdrawable, binance.blockChainIsWithdrawable, mexc.blockChainIsWithdrawable, bitget.blockChainIsWithdrawable, gateIo.blockChainIsWithdrawable, okx.blockChainIsWithdrawable, coinex.blockChainIsWithdrawable, bingX.blockChainIsWithdrawable, kucoin.blockChainIsWithdrawable };
            List<bool>[] isDepozitable = new List<bool>[] { bybit.blockChainIsDepozitable, binance.blockChainIsDepozitable, mexc.blockChainIsDepozitable, bitget.blockChainIsDepozitable, gateIo.blockChainIsDepozitable, okx.blockChainIsDepozitable, coinex.blockChainIsDepozitable, bingX.blockChainIsDepozitable, kucoin.blockChainIsDepozitable };
            List<string>[] fullNames = new List<string>[] { bybit.blockChainFullName, binance.blockChainFullName, mexc.blockChainFullName, bitget.blockChainFullName, gateIo.blockChainFullName, okx.blockChainFullName, coinex.blockChainFullName, bingX.blockChainFullName, kucoin.blockChainFullName };

            sellIndex = GetSellIndex(avgSellPrices);
            buyIndex = GetBuyIndex(avgBuyPrices);

            if (sellIndex == -1 || buyIndex == -1) return State.Error;
            try
            {
                avgBuyPrice = avgBuyPrices[buyIndex];
                lastBuyPrice = asks[buyIndex];
                buyingExchange = names[buyIndex];
                avgBuyQuantity = askQuantities[buyIndex];
                takerFeeRate = tFees[buyIndex];
                firstExchangeChains = chains[buyIndex];
                withdrawalFees = fees[buyIndex].ToArray();
                isWithdraw = isWithdrawable[buyIndex];
                chainsFullName1 = fullNames[buyIndex];

                avgSellPrice = avgSellPrices[sellIndex];
                lastSellPrice = bids[sellIndex];
                sellingExchange = names[sellIndex];
                avgSellQuantity = bidQuantities[sellIndex];
                makerFeeRate = mFees[sellIndex];
                secondExchangeChains = chains[sellIndex];
                isDepozit = isDepozitable[sellIndex];
                chainsFullName2 = fullNames[sellIndex];
            }
            catch 
            {
                return State.Error;
            }
            return State.Success;
        }
        private State CheckArbitrageSituation(string p)
        {
            string[] links = new string[]
            {
                $"https://www.bybit.com/ru-RU/trade/spot/{p.Replace("USDT",string.Empty)}/USDT",
                $"https://www.binance.com/ru/trade/{p.Replace("USDT",string.Empty)}_USDT?type=spot",
                $"https://www.mexc.com/ru-RU/exchange/{p.Replace("USDT",string.Empty)}_USDT",
                $"https://www.bitget.com/ru/spot/{p}",
                $"https://www.gate.io/ru/trade/{p.Replace("USDT",string.Empty)}_USDT",
                $"https://www.okx.com/ru/trade-spot/{p.Replace("USDT",string.Empty).ToLower()}-usdt",
                $"https://www.coinex.com/en/exchange/{p.Replace("USDT",string.Empty).ToLower()}-usdt",
                $"https://bingx.com/en/spot/{p}/",
                $"https://www.kucoin.com/trade/{p.Replace("USDT",string.Empty)}-USDT"
            };
            State state = SelectData();

            if (state == State.Success && !isWithdraw.Contains(true) || !isDepozit.Contains(true) || firstExchangeChains.Count == 0 || secondExchangeChains.Count == 0 || avgSellQuantity == 0 || avgBuyQuantity == 0 || buyingExchange == sellingExchange || firstExchangeChains.Count == 0 || secondExchangeChains.Count == 0)
            {
                return State.Error; 
            }
            try
            {
                buyLink = $"<a href=\"{links[buyIndex]}\">{buyingExchange}</a>";
                sellLink = $"<a href=\"{links[sellIndex]}\">{sellingExchange}</a>";
            }
            catch { return State.Error; }

            if (avgBuyQuantity > avgSellQuantity)
            {
                activeQuantity = avgSellQuantity;
                usdtQuantity = avgSellPrice * avgSellQuantity;
            }
            else if (avgSellQuantity > avgBuyQuantity)
            {
                activeQuantity = avgBuyQuantity;
                usdtQuantity = avgBuyPrice * avgBuyQuantity;
            }
            else if (avgSellQuantity == avgBuyQuantity)
            {
                activeQuantity = avgBuyQuantity;
                usdtQuantity = avgBuyPrice * avgBuyQuantity;
            }

            withdrawalFee = GetWithdrawalData(withdrawalFees);
            if (withdrawalFee == -1) return State.Error;
            try
            {
                spread = Math.Round(((avgSellPrice - avgBuyPrice) / avgSellPrice) * 100, 2);

                fullFee = ((activeQuantity * avgBuyPrice) * (takerFeeRate / 100)) + ((activeQuantity * avgSellPrice) * (makerFeeRate / 100)) + (withdrawalFee * avgBuyPrice);

                profit = Math.Round((activeQuantity * avgSellPrice) - (activeQuantity * avgBuyPrice) - fullFee, 4);
            }
            catch { return State.Error; }
            if (spread >= spreadRange1 && spread <= spreadRange2 && profit >= 1) 
                return State.Success;
            else return State.Error;
        }
        private string CheckState(State state)
        {
            switch (state)
            {
                case State.Error:
                    return "Непредвиденная ошибка";
                case State.SpreadLowerThanZeroError:
                    return "Отрицательный спред";
                case State.WithdrawalDataError:
                    return "Нет доступных блокчейн-сетей";
                case State.Success:
                    return "Удачно";
            }
            return string.Empty;
                    
        }
        public string GenerateMessage(string p)
        {
            GetFullData(p);
            State state = CheckArbitrageSituation(p);
            if (state == State.Error) return string.Empty;
            string message = string.Empty;
            try
            {
                message += $"📊Пара: <b><u>{p.Replace("USDT", string.Empty)}/USDT📊</u></b>\n\n";
                message += $"<b>{buyLink} --> {sellLink}</b>\n";
                message += $"📈Покупка📈\n";
                message += $"Объем: <b>{Math.Round(avgBuyQuantity, 8)} {p.Replace("USDT",string.Empty)}</b>\n";
                message += $"💵Средняя цена ≈ <b>{avgBuyPrice} USDT</b>💵\n\n";
                message += $"📉Продажа📉\n";
                message += $"Объем: <b>{Math.Round(avgSellQuantity, 8)} {p.Replace("USDT",string.Empty)}</b>\n";
                message += $"💵Средняя цена ≈ <b>{avgSellPrice} USDT</b>💵\n\n";

                message += $"Итоговый объем сделки: {usdtQuantity}$\n\n";

                message += $"🔥<u>Спред</u>: <b>{spread} %</b>\n";
                message += $"🔥<u>Профит с учетом комиссии</u>: <b>{profit} USDT</b> \n";
                message += $"<u>❗️Комиссия сделки</u>: <b>{Math.Round(fullFee, 4)} USDT❗️</b> \n";
                message += $"<b><u>⛓Сети⛓</u></b>\n";
                if (firstExchangeChains.Count == 0 && secondExchangeChains.Count != 0)
                {
                    message += $"<b>Сети на <u>{buyingExchange}</u></b>\n";
                    message += $"<b>❗️Доступных сетей нет❗️</b>\n";
                    message += $"<b>Сети на <u>{sellingExchange}</u></b>\n";
                    for (int i = 0; i < secondExchangeChains.Count; i++)
                    {
                        if (isDepozit[i] == true)
                        {
                            message += $"⚡️<u><b>{secondExchangeChains[i]}</b>, депозит есть🟢</u>\n";
                        }
                        else
                        {
                            message += $"⚡️<u><b>{secondExchangeChains[i]}</b>, депозита нет🔴</u>\n";
                        }
                    }
                }
                else if (firstExchangeChains.Count != 0 && secondExchangeChains.Count == 0)
                {
                    message += $"<b>Сети на <u>{sellingExchange}</u></b>\n";
                    for (int i = 0; i < firstExchangeChains.Count; i++)
                    {
                        if (isWithdraw[i] == true)
                        {
                            message += $"⚡️<u><b>{firstExchangeChains[i]}</b>, комиссия <b>{withdrawalFees[i]} {myTI.ToTitleCase(p.Replace("USDT", string.Empty).ToLower())}</b>, вывод есть🟢</u>\n";
                        }
                        else
                        {
                            message += $"⚡️<u><b>{firstExchangeChains[i]}</b>, комиссия <b>{withdrawalFees[i]} {myTI.ToTitleCase(p.Replace("USDT", string.Empty).ToLower())}</b>, вывода нет🔴</u>\n";
                        }
                    }
                    message += $"<b>Сети на <u>{sellingExchange}</u></b>\n";
                    message += $"<b>❗️Доступных сетей нет❗️</b>\n";
                }
                else if (firstExchangeChains.Count == 0 && secondExchangeChains.Count == 0)
                {
                    message += $"<b>Сети на <u>{buyingExchange}</u></b>\n";
                    message += $"<b>❗️Доступных сетей нет❗️</b>\n";
                    message += $"<b>Сети на <u>{sellingExchange}</u></b>\n";
                    message += $"<b>❗️Доступных сетей нет❗️</b>\n";
                }
                else if (firstExchangeChains.Count != 0 && secondExchangeChains.Count != 0)
                {
                    message += $"<b>Сети на <u>{buyingExchange}</u></b>\n";
                    for (int i = 0; i < firstExchangeChains.Count; i++)
                    {
                        if (isWithdraw[i] == true)
                        {
                            message += $"⚡️<u><b>{firstExchangeChains[i]}</b>, комиссия <b>{withdrawalFees[i]} {myTI.ToTitleCase(p.Replace("USDT", string.Empty).ToLower())}</b>, вывод есть🟢</u>\n";
                        }
                        else
                        {
                            message += $"⚡️<u><b>{firstExchangeChains[i]}</b>, комиссия <b>{withdrawalFees[i]} {myTI.ToTitleCase(p.Replace("USDT", string.Empty).ToLower())}</b>, вывода нет🔴</u>\n";
                        }
                    }
                    message += $"<b>Сети на <u>{sellingExchange}</u></b>\n";
                    for (int i = 0; i < secondExchangeChains.Count; i++)
                    {
                        if (isWithdraw[i] == true)
                        {
                            message += $"⚡️<u><b>{secondExchangeChains[i]}</b>, депозит есть🟢</u>\n";
                        }
                        else
                        {
                            message += $"⚡️<u><b>{secondExchangeChains[i]}</b>, депозита нет🔴</u>\n";
                        }
                    }
                }
                return message;
            }
            catch { return string.Empty; }
        }
    }
}
