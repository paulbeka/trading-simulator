namespace MarketDataService.Models
{
    public class MarketDataMessage
    {
        public string Ticker { get; set; } = "";
        public decimal Price { get; set; }
    }
}
