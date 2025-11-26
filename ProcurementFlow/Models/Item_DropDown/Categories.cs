using System.ComponentModel.DataAnnotations;

namespace ProcurementFlow.Models.Common_Pages
{
    public class Categories
    {
        [Key]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
