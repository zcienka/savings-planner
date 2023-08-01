﻿using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAll();
        Task<Transaction> GetByIdAsync(string id);
        int Add(Transaction transaction);
        int Update(Transaction transaction);
        int Delete(Transaction transaction);
        int Save();
        bool Exists(string id);
        List<float> GetTransactionListByCurrentMonth(string type, string userId);
        List<string> GetCategoriesByCurrentMonth(string type, string userId);
        List<MonthlyIncomeAndExpenses> GetIncomeAndExpensesPast12Months(string userId);
        float GetSumByCurrentMonth(string type, string userId);
        Task<IEnumerable<Transaction>> GetAllByUserId(string userId);
    }
}