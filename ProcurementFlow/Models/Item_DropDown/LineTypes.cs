using System.ComponentModel.DataAnnotations;

namespace ProcurementFlow.Models.Common_Pages
{
    public class LineTypes
    {
        [Key]
        public int LineTypeId { get; set; }
        public string? LineTypeName { get; set; }
    }
}
