using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcurementFlow.Models.Common_Pages
{
    public class UOM
    {
        [Key]
        public int Id { get; set; }
        public string? UOMName {  get; set; }
    }
}
