using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Models
{
    public class TransactionType
    {
        [Key]
        public string Name { get; set; }
    }
}