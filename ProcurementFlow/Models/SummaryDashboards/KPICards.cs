namespace ProcurementFlow.Models.SummaryDashboards
{
    public class KPICards
    {
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public double TotalSpend { get; set; }
        public double UrgentSpend { get; set; }
    }
}
