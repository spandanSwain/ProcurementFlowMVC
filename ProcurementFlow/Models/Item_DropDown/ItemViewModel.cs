using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProcurementFlow.Models.Item_DropDown
{
    public class ItemViewModel
    {
        public List<SelectListItem> LineType { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> UOM { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public List<NonCatalogDisplayItems> NonCatalogItems { get; set; } = new List<NonCatalogDisplayItems>();

        [Range(1, 150000, ErrorMessage = "Please enter a price range between ₹1 - ₹1,50,000")]
        public double Price { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public string? SelectedLineType { get; set; }
        public string? SelectedCategory { get; set; }
        public string? SelectedUOM { get; set; }
        public string? UpdatedBy { get; set; }
        public string ItemType { get; set; } = "non-catalog";
        [DataType(DataType.Date)]
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
