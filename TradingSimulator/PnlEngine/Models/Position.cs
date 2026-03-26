namespace PnlEngine.Models
{
    public class Position
    {
        public string Ticker { get; set; } = default!;
        public decimal Quantity { get; set; }
        public decimal AvgEntryPrice { get; set; }
    }
}
