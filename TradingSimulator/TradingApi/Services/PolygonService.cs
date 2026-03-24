using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TradingApi.Contracts.Polygon;
using TradingApi.Options;
using TradingApi.Services.Interfaces;

public class PolygonService : IPolygonService
{
    private readonly HttpClient _httpClient;
    private readonly PolygonConfig _options;
    private readonly IMemoryCache _cache;

    public PolygonService(
        HttpClient httpClient,
        IOptions<PolygonConfig> options,
        IMemoryCache cache)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _cache = cache;
    }

    public async Task<List<PolygonTicker>> SearchTickersAsync(string query)
    {
        var cacheKey = $"ticker_search_{query.ToLower()}";

        if (_cache.TryGetValue(cacheKey, out List<PolygonTicker> cached))
        {
            return cached;
        }

        var url = $"https://api.polygon.io/v3/reference/tickers" +
                  $"?search={query}&active=true&type=CS&limit=20&apiKey={_options.ApiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<PolygonResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var tickers = result?.results ?? new List<PolygonTicker>();

        _cache.Set(cacheKey, tickers, TimeSpan.FromMinutes(30));

        return tickers;
    }

    private class PolygonResponse
    {
        public List<PolygonTicker> results { get; set; }
    }
}