namespace TradingApi.Contracts.Trading
{
    public class TradeResponse
    {
        public required string Ticker { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public required string Side { get; set; }
        public decimal RemainingCash { get; set; }
    }
}
