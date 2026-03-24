using TradingApi.Contracts.Polygon;

namespace TradingApi.Services.Interfaces
{
    public interface IPolygonService
    {
        Task<List<PolygonTicker>> SearchTickersAsync(string query);
    }
}
