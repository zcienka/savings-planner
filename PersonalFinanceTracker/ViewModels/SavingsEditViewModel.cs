using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.ViewModels
{
    public class SavingsEditViewModel
    {
        public Savings Savings { get; set; }

        public IEnumerable<SavingsStatus> Status { get; set; }

        public SavingsEditViewModel()
        {
            Savings = new Savings();
            Status = new List<SavingsStatus>();
        }
    }
}
