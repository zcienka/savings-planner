using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Interfaces;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Repository
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _context;

        public BudgetRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Budget budget)
        {
            _context.Add(budget);
            return Save();
        }

        public bool Delete(Budget budget)
        {
            _context.Remove(budget);
            return Save();
        }

        public async Task<IEnumerable<Budget>> GetAll(string userId)
        {
            return await _context.BudgetContext.Where(b=> b.UserId == userId).ToListAsync();
        }

        public async Task<Budget> GetByIdAsync(string id)
        {
            return await _context.BudgetContext.FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Budget budget)
        {
            _context.Update(budget);
            return Save();
        }

        public bool Exists(string id)
        {
            return (_context.BudgetContext?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
