using Nancy.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Helpers
{
    public static class Requests
    {
        private const string API_KEY_COINMARKETCAP = "06d7666b-c179-4097-8ee7-d52c1cde0963";
        private const string API_KEY_COINGLASS = "9946f82d2fb84dd396940f3c6f6d059b";
        public static JObject GetRequestCoinMarketCap(string url)
        {
            var URL = new UriBuilder(url);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = "100";
            queryString["convert"] = "USD";

            URL.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY_COINMARKETCAP);
            client.Headers.Add("Accepts", "application/json");
            JObject json = JObject.Parse(client.DownloadString(URL.ToString()));
            return json;
        }
        public static JObject GetRequestCoinGlass(string symbol)
        {
            var URL = new UriBuilder("https://open-api.coinglass.com/public/v2/perpetual_market");

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["symbol"] = symbol;
            queryString["interval"] = "24H";

            URL.Query = queryString.ToString();
            

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY_COINGLASS);
            client.Headers.Add("Accepts", "application/json");
            JObject json = JObject.Parse(client.DownloadString(URL.ToString()));
            return json;
        }
    }
}
