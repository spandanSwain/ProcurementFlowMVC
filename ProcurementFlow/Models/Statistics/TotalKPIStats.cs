namespace ProcurementFlow.Models.Statistics
{
    public class TotalKPIStats
    {
        public int TotalRequisitions { get; set; }
        public int TotalApproved { get; set; }
        public int TotalRejected { get; set; }
        public double TotalSpent { get; set; }
        public decimal AverageDelay { get; set; }
    }
}
