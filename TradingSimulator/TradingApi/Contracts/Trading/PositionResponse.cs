namespace TradingApi.Contracts.Trading
{
    public class PositionResponse
    {
        public string Ticker { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal AvgEntryPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
