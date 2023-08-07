using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Dtos
{
    public class TransactionDto
    {
        public string Category { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }
}
