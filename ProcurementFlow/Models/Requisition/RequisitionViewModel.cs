using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProcurementFlow.Models.Requisition
{
    public class RequisitionViewModel
    {
        public int SelectedItemID { get; set; }
        public List<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        // FROM FORM
        public string Status { get; set; } = "Pending";
        public int Urgent { get; set; }
        public int Quantity { get; set; }
        public string? Requestor { get; set; }
        [DataType(DataType.Date)]
        public DateTime? UpdatedOn { get; set; }
        public double CalculatedPrice { get; set; }
        public string? DeliveryAddress { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string? DeliveryLocation { get; set; }
        public int TotalRequisition { get; set; } // dashboard
        public int ApprovedRequisition { get; set; } // dashboard
        public int RejectedRequisition { get; set; } // dashboard
        public PaginatedList<RequisitionListItems>? RequisitionListItems { get; set; }// Requisition table
        public string? Action { get; set; } // insert edit delete
        public int SelectedRequisitionItemId { get; set; } // for edit and delete
    }
}
