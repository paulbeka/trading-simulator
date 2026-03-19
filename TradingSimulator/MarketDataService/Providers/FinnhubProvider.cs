using System.Text.Json;

namespace MarketDataService.Providers
{
    internal class FinnhubProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string BASE_URL = "https://finnhub.io/api/v1";
        private readonly string API_KEY = Environment.GetEnvironmentVariable("FINNHUB_API")
            ?? throw new Exception("FINNHUB_API not set");

        public FinnhubProvider()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<(string ticker, decimal price)>> GetPricesAsync(string[] tickers)
        {
            var prices = new List<(string, decimal)>();

            foreach (var ticker in tickers)
            {
                var url = $"{BASE_URL}/quote?symbol={ticker}&token={API_KEY}";

                var response = await _httpClient.GetStringAsync(url);
                var json = JsonDocument.Parse(response);
                Console.WriteLine(json);

                var currentPrice = json.RootElement.GetProperty("c").GetDecimal();

                prices.Add((ticker, currentPrice));
            }

            return prices;
        }
    }
}