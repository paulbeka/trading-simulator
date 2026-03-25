using Database.Enums;

namespace Database.Entities
{
    public class Trade
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string Ticker { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public TradeSide Side { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
