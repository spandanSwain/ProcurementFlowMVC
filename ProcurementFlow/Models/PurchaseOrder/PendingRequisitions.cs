using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace ProcurementFlow.Models.PurchaseOrder
{
    public class PendingRequisitions
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? Urgent { get; set; }
        [Display(Name = "Priority Status")]
        public int PriorityStatus { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "Price")]
        public double CalculatedPrice { get; set; }
        [Display(Name = "Original Price")]
        public double OriginalPrice { get; set; }
        [Display(Name = "Requested By")]
        public string? RequestedBy { get; set; }
        [Display(Name = "Delivery Address")]
        public string? DeliveryAddress { get; set; }
        [Display(Name = "Delivery Location")]
        public string? DeliveryLocation { get; set; }
        [Display(Name = "Requested Date")]
        public DateTime? RequestedDate { get; set; }
        [DataType(DataType.Date), Display(Name = "Last Updated on")]
        public DateTime? UpdatedOn { get; set; }
        [Display(Name = "Category")]
        public string? CategoryName { get; set; }
        [Display(Name = "UoM")]
        public string? UOM { get; set; }
        [Display(Name = "Line Type")]
        public string? LineTypeName { get; set; }
        [Display(Name = "Item Name")]
        public string? ItemName { get; set; }
        [Display(Name = "Item Description")]
        public string? ItemDescription { get; set; }
    }
}
