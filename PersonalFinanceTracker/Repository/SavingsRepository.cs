using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Interfaces;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Repository
{
    public class SavingsRepository : ISavingsRepository
    {
        private readonly ApplicationDbContext _context;

        public SavingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Savings savings)
        {
            _context.Add(savings);
            return Save();
        }

        public bool Delete(Savings savings)
        {
            _context.Remove(savings);
            return Save();
        }

        public async Task<IEnumerable<Savings>> GetAll()
        {
            return await _context.Savings.ToListAsync();
        }

        public async Task<IEnumerable<Savings>> GetAllByUserId(string userId)
        {
            return await _context.Savings
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.Deadline)
                .ToListAsync();
        }
        public async Task<Savings> GetByIdAsync(string id)
        {
            return await _context.Savings.FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Savings savings)
        {
            _context.Update(savings);
            return Save();
        }

        public bool Exists(string id)
        {
            return (_context.Savings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
