namespace Database.Entities
{
    public sealed class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public ICollection<Trade> Trades { get; set; } = new List<Trade>();

        public ICollection<Position> Positions { get; set; } = new List<Position>();

        public Account? Account { get; set; }
    }
}
