using System.ComponentModel.DataAnnotations.Schema;

namespace ProcurementFlow.Models.SummaryDashboards
{    
    public class SummaryDashboardViewModel
    {
        [NotMapped]
        public KPICards? KPICards { get; set; }
        [NotMapped]
        public List<CategorySpend> CategorySpend {  get; set; } = new List<CategorySpend>();
        [NotMapped]
        public List<ManagerApproveRejectBarChart> ManagerApproveRejectBarChart { get; set; } = new List<ManagerApproveRejectBarChart>();
    }
}
