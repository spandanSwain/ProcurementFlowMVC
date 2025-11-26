using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace ProcurementFlow.Models.Statistics
{
    public class SpendPerCategory
    {
        public string? CategoryName { get; set; }
        public double CategorySpending { get; set; }
    }
}
