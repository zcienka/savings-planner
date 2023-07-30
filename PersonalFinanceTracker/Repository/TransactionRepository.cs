using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Interfaces;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(Transaction transaction)
        {
            _context.Add(transaction);
            return Save();
        }

        public int Delete(Transaction transaction)
        {
            _context.Remove(transaction);
            return Save();
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(string id)
        {
            return await _context.Transactions.FirstOrDefaultAsync(i => i.Id == id);
        }

        public int Save()
        {
            var saved = _context.SaveChanges();
            return saved;
        }

        public int Update(Transaction transaction)
        {
            _context.Update(transaction);
            return Save();
        }
        public bool Exists(string id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public List<float> GetTransactionListByCurrentMonth(string type)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now;

            List<float> transactions = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == type)
                .GroupBy(t => t.Category)
                .Select(t => t.Sum(t => t.Amount))
                .ToList();
            return transactions;
        }

        public List<string> GetCategoriesByCurrentMonth(string type)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now;
            List<string> categories = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == type)
                .GroupBy(t => t.Category)
                .Select(t => t.Key)
                .ToList();
            return categories;
        }
        public List<MonthlyIncomeAndExpenses> GetIncomeAndExpenses()
        {
            List<MonthlyIncomeAndExpenses> incomeAndExpenses = _context.Transactions
                .GroupBy(t => new { Month = t.Date.Month, Year = t.Date.Year })
                .Select(group => new MonthlyIncomeAndExpenses
                {
                    Month = group.Key.Month,
                    Year = group.Key.Year,
                    Income = group.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    Expenses = group.Where(t => t.Type == "Expense").Sum(t => t.Amount)
                })
                .OrderBy(group => group.Year)
                .ThenBy(group => group.Month)
                .ToList();

            return incomeAndExpenses;
        }

        public float GetSumByCurrentMonth(string type)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now;

            var currentMonthIncome = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == type)
                .ToList()
                .Sum(t => t.Amount);
            return currentMonthIncome;
        }
    }
}
