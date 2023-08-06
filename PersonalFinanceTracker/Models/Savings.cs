using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Models
{
    public class Savings
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Target Amount")]
        public decimal TargetAmount { get; set; }
        [Display(Name = "Current Amount")]
        public decimal CurrentAmount { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
    }
}