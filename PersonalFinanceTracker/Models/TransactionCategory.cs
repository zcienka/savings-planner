using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Models
{
    public class TransactionCategory
    {
        [Key]
        public string Name { get; set; }
    }
}
