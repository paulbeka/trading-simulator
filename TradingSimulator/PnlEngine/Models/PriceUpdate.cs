namespace PnlEngine.Models
{
    internal class PriceUpdate
    {
        public required string Ticker { get; set; }

        public required decimal Price { get; set; }
    }
}
