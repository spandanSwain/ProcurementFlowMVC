using System.ComponentModel.DataAnnotations.Schema;

namespace ProcurementFlow.Models.Requisition
{
    public class ItemDetails
    {
        public string? UOM { get; set; }
        public double Price { get; set; }
        [Column("LineTypeName")]
        public string? LineType { get; set; }
        public string? ItemType { get; set; }
        public string? CategoryName { get; set; }
        public string? ItemDescription { get; set; }
    }
}
