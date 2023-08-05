using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Interfaces
{
    public interface ISavingsRepository
    {
        Task<IEnumerable<Savings>> GetAll();
        Task<IEnumerable<Savings>> GetAllByUserId(string userId);
        Task<Savings> GetByIdAsync(string id);
        bool Add(Savings savings);
        bool Update(Savings savings);
        bool Delete(Savings savings);
        bool Save();
        bool Exists(string id);
    }
}
