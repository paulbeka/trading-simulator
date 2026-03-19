using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace MarketDataService.Providers
{
    internal class YahooFinanceProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string BASE_URL = "https://query1.finance.yahoo.com";

        public YahooFinanceProvider()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<(String ticker, decimal price)>> GetPricesAsync(string[] tickers)
        {
            var symbolString = string.Join(",", tickers);
            var url = $"{BASE_URL}/v7/finance/quote?symbols={symbolString}";

            var response = await _httpClient.GetStringAsync(url);
            var json = JsonDocument.Parse(response);

            var results = json
                .RootElement
                .GetProperty("quoteResponse")
                .GetProperty("result");

            var prices = new List<(string, decimal)>();

            foreach (var item in results.EnumerateArray())
            {
                var symbol = item.GetProperty("symbol").GetString()!;
                var price = item.GetProperty("regularMarketPrice").GetDecimal();

                prices.Add((symbol, price));
            }

            return prices;
        }
    }
}
