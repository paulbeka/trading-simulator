namespace PnlEngine.Models
{
    public class Position
    {
        public string Ticker { get; set; } = default!;
        public decimal Quantity { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal LastPrice { get; set; }
    }
}
