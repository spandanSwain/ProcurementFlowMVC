using Microsoft.EntityFrameworkCore;
using ProcurementFlow.Data;
using ProcurementFlow.Models.Login;

namespace ProcurementFlow.DataAccess
{
    public class Login
    {
        private readonly ProcurementFlowContext _context;
        public Login(ProcurementFlowContext context)
        {
            _context = context;
        }
        public async Task<(string?, string?, int, int)> ValidateUsersAsync(string userName, string passWord)
        {
            var data = await _context.Set<LoginResult>()
                               .FromSqlInterpolated($"EXECUTE SP_VALIDATE_USERS @V_USERNAME = {userName}, @V_PASSWORD = {passWord}")
                               .AsNoTracking().ToListAsync();
            var result = data.FirstOrDefault();
            return (result?.Name ?? "", result?.Role ?? "", result?.ResultCount ?? 0, result?.Id ?? 0);
        }
    }
}
