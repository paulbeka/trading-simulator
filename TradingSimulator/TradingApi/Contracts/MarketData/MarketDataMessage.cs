namespace TradingApi.Contracts.MarketData
{
    public class MarketDataMessage
    {
        public string Ticker { get; set; } = "";
        public decimal Price { get; set; }
    }
}
