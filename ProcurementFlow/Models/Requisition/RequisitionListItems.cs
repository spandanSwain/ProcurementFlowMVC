using System.ComponentModel.DataAnnotations;

namespace ProcurementFlow.Models.Requisition
{
    public class RequisitionListItems
    {
        public int Id { get; set; }
        [Display(Name = "Item Name")]
        public string? RequisitionLineItemName { get; set; }
        [DataType(DataType.Date), Display(Name = "Last Updated on")]
        public DateTime UpdatedOn { get; set; }
        [Display(Name = "Requested on")]
        public string? RequestedDate { get; set; }
    }
}
