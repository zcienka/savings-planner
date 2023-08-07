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
            return await _context.Transactions
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllByUserId(string userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
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

        public List<decimal> GetTransactionListByCurrentMonth(string type, string userId)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now;

            List<decimal> transactions = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == type &&
                            t.UserId == userId)
                .GroupBy(t => t.Category)
                .Select(t => t.Sum(t => t.Amount))
                .ToList();
            return transactions;
        }

        public List<string> GetCategoriesByCurrentMonth(string type, string userId)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now;
            List<string> categories = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == type &&
                            t.UserId == userId)
                .GroupBy(t => t.Category)
                .Select(t => t.Key)
                .ToList();
            return categories;
        }

        public List<MonthlyIncomeAndExpenses> GetIncomeAndExpensesPast12Months(string userId)
        {
            DateTime currentDate = DateTime.Now;
            DateTime firstDayOfNextMonth = new DateTime(currentDate.Year, currentDate.Month + 1, 1);
            DateTime twelveMonthsAgo = firstDayOfNextMonth.AddMonths(-12);

            List<MonthlyIncomeAndExpenses> incomeAndExpenses = _context.Transactions
                .Where(t => t.Date > twelveMonthsAgo && t.Date < firstDayOfNextMonth && t.UserId == userId)
                .GroupBy(t => new { Year = t.Date.Year, Month = t.Date.Month })
                .Select(group => new MonthlyIncomeAndExpenses
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    Income = group.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    Expenses = group.Where(t => t.Type == "Expense").Sum(t => t.Amount)
                })
                .OrderBy(group => group.Year)
                .ThenBy(group => group.Month)
                .ToList();

            return incomeAndExpenses;
        }

        public decimal GetSumByCurrentMonth(string type, string userId)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now;

            var currentMonthIncome = _context.Transactions
                .Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month && t.Type == type &&
                            t.UserId == userId)
                .ToList()
                .Sum(t => t.Amount);
            return currentMonthIncome;
        }

        public async Task<IEnumerable<TransactionCategory>> GetAllCategories()
        {
            return await _context.TransactionCategories.ToListAsync();
        }

        public async Task<IEnumerable<TransactionType>> GetAllTypes()
        {
            return await _context.TransactionTypes.ToListAsync();
        }
    }
}