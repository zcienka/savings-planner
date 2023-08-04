namespace PersonalFinanceTracker.Models
{
    public class Savings
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string UserId { get; set; }
        public float TargetAmount { get; set; }
        public float CurrentAmount { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public string Status { get; set; }
    }
}