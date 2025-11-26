using System.ComponentModel.DataAnnotations;

namespace ProcurementFlow.Models.PurchaseOrder
{
    public class RejectionItemReceipt
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Status { get; set; }
        public string? Urgent { get; set; }
        public string? ItemName { get; set; }
        public string? Comments { get; set; }
        public string? Requestor { get; set; }
        public string? IsSameUser { get; set; }
        [DataType(DataType.Date)]
        public string? RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
        public string? RequestorName { get; set; }
        public string? RequestorRole { get; set; }
        public string? RejectedByRole { get; set; }
        public string? RejectedByName { get; set; }
        public string? ItemDescription { get; set; }
        public DateTime? RequestedDate { get; set; }
    }
}
