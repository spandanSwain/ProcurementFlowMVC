using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace ProcurementFlow.Models.TranscationConsole
{
    public class RequisitionTransactions
    {
        public int Id { get; set; }
        [Display(Name = "Item")]
        public string? ItemName { get; set; }
        [Display(Name = "Item Description")]
        public string? ItemDescription { get; set; }
        [Display(Name = "Item Type")]
        public string? ItemType { get; set; }
        [Display(Name = "Status")]
        public string? Status { get; set; }
        [Display(Name = "Urgent")]
        public string? Urgent {  get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Price")]
        public double Price { get; set; }
        [Display(Name = "Delivery Address")]
        public string? DeliveryAddress { get; set; }
        [Display(Name = "Delivery Location")]
        public string? DeliveryLocation { get; set; }
        [DataType(DataType.Date), Display(Name = "Action Taken On")]
        public DateTime? ActionTakenOn { get; set; }
        [Display(Name = "Line Type")]
        public string? LineTypeName { get; set; }
        [Display(Name = "UoM")]
        public string? UOM { get; set; }
        [Display(Name = "Category")]
        public string? CategoryName { get; set; }
        [Display(Name = "Action Taken By")]
        public string? ActionBy { get; set; }
        [Display(Name = "Requested Date")]
        public DateTime? RequestedDate { get; set; }
    }
}
