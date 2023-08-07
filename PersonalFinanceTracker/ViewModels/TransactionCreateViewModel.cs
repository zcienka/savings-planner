using PersonalFinanceTracker.Dtos;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.ViewModels
{
    public class TransactionCreateViewModel
    {
        public TransactionDto TransactionDto { get; set; }

        public IEnumerable<TransactionCategory> Categories { get; set; }
        public IEnumerable<TransactionType> Types { get; set; }

        public TransactionCreateViewModel()
        {
            TransactionDto = new TransactionDto();
            Categories = new List<TransactionCategory>();
            Types = new List<TransactionType>();
        }

    }
}
