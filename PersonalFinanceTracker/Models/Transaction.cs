namespace PersonalFinanceTracker.Models
{
    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string UserId { get; set; }
        public string Category { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
    }
}