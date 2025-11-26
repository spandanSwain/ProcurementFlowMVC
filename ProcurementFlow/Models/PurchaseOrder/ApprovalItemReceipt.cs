using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProcurementFlow.Models.PurchaseOrder
{
    public class ApprovalItemReceipt
    {
        public int Id { get; set; }
        public string? Approver { get; set; }
        public string? ApproverRole { get; set; }
        public string? ApproverName { get; set; }
        public string? Requestor { get; set; }
        public string? RequestorRole { get; set; }
        public string? RequestorName { get; set; }

        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Status { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? DeliveryLocation { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string? Urgent { get; set; }
        public string? IsSameUser { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public double OriginalPrice { get; set; }
        public string? LineTypeName { get; set; }
        public string? UOM { get; set; }
        public string? CategoryName { get; set; }
        [NotMapped]
        public string? SupplierName { get; set; } = "Dummy Supplier Ltd.";
        [NotMapped]
        public string? SupplierPhone { get; set; } = "1234567890";
        [NotMapped]
        public string? SupplierEmail { get; set; } = "spandanswain2003s@gmail.com";
    }
}
