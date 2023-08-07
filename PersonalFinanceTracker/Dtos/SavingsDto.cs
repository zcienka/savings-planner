using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Dtos
{
    public class SavingsDto
    {
        public string Title { get; set; }
        public DateOnly Date { get; set; }
        [Display(Name = "Target Amount")]
        public decimal TargetAmount { get; set; }
        [Display(Name = "Current Amount")]
        public decimal CurrentAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateOnly Deadline { get; set; }
        public string Status { get; set; }
    }
}
