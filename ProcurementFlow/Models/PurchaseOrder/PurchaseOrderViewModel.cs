namespace ProcurementFlow.Models.PurchaseOrder
{
    public class PurchaseOrderViewModel
    {
        public PaginatedList<PendingRequisitions>? PendingRequisitions { get; set; }
        public int User { get; set; }
        public int SelectedId { get; set; }
        public string? Action { get; set; }
        public string? Comment { get; set; }
    }
}
