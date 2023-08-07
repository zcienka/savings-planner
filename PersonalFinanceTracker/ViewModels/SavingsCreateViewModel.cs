using PersonalFinanceTracker.Dtos;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.ViewModels
{
    public class SavingsCreateViewModel
    {
        public SavingsDto SavingsDto { get; set; }

        public IEnumerable<SavingsStatus> Status { get; set; }

        public SavingsCreateViewModel()
        {
            SavingsDto = new SavingsDto();
            Status = new List<SavingsStatus>();
        }
    }
}