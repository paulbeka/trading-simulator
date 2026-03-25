namespace TradingApi.Contracts.Trading
{
    public class TradeRequest
    {
        public required string Ticker { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public required string Side { get; set; }
    }
}
