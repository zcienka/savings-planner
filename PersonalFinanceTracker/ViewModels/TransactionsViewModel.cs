using PersonalFinanceTracker.Dtos;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.ViewModels
{
    public class TransactionsViewModel
    {
        public int SelectedCategoryId { get; set; }

        public TransactionDto TransactionDto;

        public Transaction Transaction;
    }
}