using System.Text.Json;

namespace MarketDataService.Providers
{
    internal class PolygonProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string BASE_URL = "https://api.polygon.io";
        private readonly string API_KEY = Environment.GetEnvironmentVariable("POLYGON_API")
            ?? throw new Exception("POLYGON_API not set");

        public PolygonProvider()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<(string ticker, decimal price)>> GetPricesAsync(string[] tickers)
        {
            var prices = new List<(string, decimal)>();

            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            foreach (var ticker in tickers)
            {
                try
                {
                    var url = $"{BASE_URL}/v2/aggs/ticker/{ticker}/range/1/minute/{today}/{today}?apiKey={API_KEY}";
                    
                    var response = await _httpClient.GetStringAsync(url);

                    using var json = JsonDocument.Parse(response);

                    if (json.RootElement.TryGetProperty("results", out var resultsArray) &&
                        resultsArray.ValueKind == JsonValueKind.Array &&
                        resultsArray.GetArrayLength() > 0)
                    {
                        var lastBar = resultsArray[resultsArray.GetArrayLength() - 1];

                        if (lastBar.TryGetProperty("c", out var closeElement))
                        {
                            var price = closeElement.GetDecimal();
                            prices.Add((ticker, price));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error for {ticker}: {ex.Message}");
                }
            }

            return prices;
        }
    }
}