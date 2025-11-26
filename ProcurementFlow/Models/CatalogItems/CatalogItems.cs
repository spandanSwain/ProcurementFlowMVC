using System.ComponentModel.DataAnnotations;

namespace ProcurementFlow.Models.CatalogItems
{
    public class CatalogItems
    {
        [Display(Name = "Line Type")]
        public string? LineTypeName { get; set; }
        [Display(Name = "Item Name")]
        public string? ItemName { get; set; }
        [Display(Name = "Item Description")]
        public string? ItemDescription { get; set; }
        [Display(Name = "Category")]
        public string? CategoryName { get; set; }
        [Display(Name = "UoM")]
        public string? UOM { get; set; }
        [Display(Name = "Price")]
        public double Price { get; set; }
    }
}
