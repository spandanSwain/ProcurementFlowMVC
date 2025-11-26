using System.ComponentModel.DataAnnotations.Schema;

namespace ProcurementFlow.Models.Statistics
{
    public class MonthlyTrend
    {
        [Column("MONTH")]
        public string? Month { get; set; }
        public double MonthlySpend { get; set; }
    }
}
