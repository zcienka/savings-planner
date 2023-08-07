using PersonalFinanceTracker.Dtos;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.ViewModels
{
    public class TransactionEditViewModel
    {
        public Transaction Transaction { get; set; }

        public IEnumerable<TransactionCategory> Categories { get; set; }
        public IEnumerable<TransactionType> Types { get; set; }

        public TransactionEditViewModel()
        {
            Transaction = new Transaction();
            Categories = new List<TransactionCategory>();
            Types = new List<TransactionType>();
        }
    }
}
