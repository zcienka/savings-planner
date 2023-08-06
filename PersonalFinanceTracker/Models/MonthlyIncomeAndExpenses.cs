namespace PersonalFinanceTracker.Models
{
    public class MonthlyIncomeAndExpenses
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
    }
}