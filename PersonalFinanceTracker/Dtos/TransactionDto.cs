namespace PersonalFinanceTracker.Dtos
{
    public class TransactionDto
    {
        public string Category { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }
}
