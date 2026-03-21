namespace PnlEngine.Models
{
    public class PnLUpdate
    {
        public required string User { get; set; }

        public required string Ticker { get; set; }

        public decimal Price { get; set; }

        public decimal PositionPnL { get; set; }

        public decimal PositionDelta { get; set; }

        public decimal TotalPnL { get; set; }

        public decimal TotalDelta { get; set; }
    }
}
