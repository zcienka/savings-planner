using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Interfaces
{
    public interface IBudgetRepository
    {
        Task<IEnumerable<Budget>> GetAll(string userId);
        Task<Budget> GetByIdAsync(string id);
        bool Add(Budget budget);
        bool Update(Budget budget);
        bool Delete(Budget budget);
        bool Save();
        bool Exists(string id);
    }
}
