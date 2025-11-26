using System.Globalization;

namespace ProcurementFlow.Models.Login
{
    public class LoginResult
    {
        public int Id { get; set; }
        public int ResultCount { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
    }
}
