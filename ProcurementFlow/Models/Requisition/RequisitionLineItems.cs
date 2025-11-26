namespace ProcurementFlow.Models.Requisition
{
    public class RequisitionLineItems
    {
        public int Item { get; set; }
        public string? ItemName { get; set; }
        public string? LineTypeName { get; set; }
        public string? ItemDescription { get; set; }
        public string? CategoryName { get; set; }
        public string? UOM { get; set; }
        public int Quantity { get; set; }
        public double CalculatedPrice { get; set; }
        public double OriginalPrice { get; set; }
        public int Urgent { get; set; }
        public DateTime RequestedDate { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? DeliveryLocation { get; set; }
    }
}
