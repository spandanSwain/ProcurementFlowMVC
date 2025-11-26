using System.ComponentModel.DataAnnotations.Schema;

namespace ProcurementFlow.Models.Statistics
{
    public class StatisticsViewModel
    {
        [NotMapped]
        public TotalKPIStats? TotalKPIStats { get; set; }
        
        [NotMapped]
        public PurchaseOrderPipeline? PurchaseOrderPipeline { get; set; }

        [NotMapped]
        public List<MonthlyTrend> MonthlyTrend { get; set; } = new List<MonthlyTrend>();

        [NotMapped]
        public List<TopManagers> TopManagers { get; set; } = new List<TopManagers>();

        [NotMapped]
        public List<SpendPerCategory> SpendPerCategory { get; set; } = new List<SpendPerCategory>();

        [NotMapped]
        public List<TopRequisitions> TopRequisitions { get; set; } = new List<TopRequisitions>();
    }
}
