using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Models
{
    public class SavingsStatus
    {
        [Key]
        public string Name { get; set; }
    }
}