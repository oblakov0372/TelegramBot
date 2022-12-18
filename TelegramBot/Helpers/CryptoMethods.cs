using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Models;

namespace TelegramBot.Helpers
{
    public static class CryptoMethods
    {

        public static string GetPercentChange(bool minusProcent)
        {
            var json = Requests.GetRequestCoinMarketCap("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");
            List<CryptoPercentChange> cryptoPercentChanges;
            string result = "";

            if (minusProcent)
                cryptoPercentChanges = CryptoPercentChanges(json).Where(o => o.PercentChange24H < 0.1M)
                                                                 .OrderBy(o => o.PercentChange24H).Take(10).ToList();
            else
                cryptoPercentChanges = CryptoPercentChanges(json).Where(o => o.PercentChange24H > 0.1M)
                                                           .OrderByDescending(o => o.PercentChange24H).Take(10).ToList();

            foreach (var item in cryptoPercentChanges)
            {
                result += $"\"{item.Name}\"\nPrice: {item.Price}\nPercentChange 24H: {item.PercentChange24H}%\nVolumeChange 24H: {item.VolumeChange24H}% \n\n ";
            }
            return result;
        }
        private static List<CryptoPercentChange> CryptoPercentChanges(JObject json)
        {
            List<CryptoPercentChange> cryptoPercentChanges = new List<CryptoPercentChange>();
            foreach (var item in json["data"])
            {
                cryptoPercentChanges.Add(new CryptoPercentChange()
                {
                    Name = item["name"].ToString(),
                    Price = Math.Round(Convert.ToDecimal(item["quote"]["USD"]["price"]), 3),
                    PercentChange24H = Math.Round(Convert.ToDecimal(item["quote"]["USD"]["percent_change_24h"]), 3),
                    VolumeChange24H = Math.Round(Convert.ToDecimal(item["quote"]["USD"]["volume_change_24h"]), 3),
                });
            }
            return cryptoPercentChanges;
        }
        public static string GetPercentShortLong(string symbol)
        {
            var json = Requests.GetRequestCoinGlass(symbol);
            decimal avarageShort = 0, avarageLong = 0;
            decimal count = json["data"][symbol].Count();
            foreach (var item in json["data"][symbol])
            {
                avarageShort += Decimal.Parse(item["shortRate"].ToString());
                avarageLong += Decimal.Parse(item["longRate"].ToString());
            }
            return $"{symbol}\nShort Rate = {Math.Round(avarageShort / count,3)}\nLong Rate = {Math.Round(avarageLong / count,3)}";
        }
    }
}
