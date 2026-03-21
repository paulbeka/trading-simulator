
namespace Database.Entities
{
    public sealed class Account
    {
        public Guid UserId { get; set; }

        public decimal CashBalance { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}
