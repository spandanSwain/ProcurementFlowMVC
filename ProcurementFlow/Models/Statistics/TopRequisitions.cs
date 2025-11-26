using System.ComponentModel.DataAnnotations;

namespace ProcurementFlow.Models.Statistics
{
    public class TopRequisitions
    {
        [Display(Name = "Req Id")]
        public string? ReqId { get; set; }
        [Display(Name = "Requested By")]
        public string? Requester { get; set; }
        public string? Category { get; set; }
        public double Value { get; set; }
        public string? Status { get; set; }
    }
}
