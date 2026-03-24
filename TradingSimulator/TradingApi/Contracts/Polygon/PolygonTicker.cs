namespace TradingApi.Contracts.Polygon
{
    public class PolygonTicker
    {
        public required string ticker { get; set; }
        public required string name { get; set; }
        public required string primary_exchange { get; set; }
        public required string type { get; set; }
    }
}
