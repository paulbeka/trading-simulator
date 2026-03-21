namespace Database.Entities
{
    public sealed class Position
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Symbol { get; set; } = null!;

        public decimal Quantity { get; set; }

        public decimal AvgEntryPrice { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}
