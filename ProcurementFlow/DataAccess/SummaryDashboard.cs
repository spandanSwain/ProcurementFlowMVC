using Dapper;
using System.Data;
using ProcurementFlow.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProcurementFlow.Models.SummaryDashboards;
using ProcurementFlow.Models.Requisition;

namespace ProcurementFlow.DataAccess
{
    public class SummaryDashboard
    {
        private readonly ProcurementFlowContext _context;
        public SummaryDashboard(ProcurementFlowContext context) { _context = context; }

        public async Task<SummaryDashboardViewModel> LoadSummaryDashboardAsync(string userId)
        {
            using var connection = _context.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            
            var multi = await connection.QueryMultipleAsync(
            "SP_SUMMARY_DASHBOARD",
                new { V_USER = Convert.ToInt32(userId) },
                commandType: CommandType.StoredProcedure
            );

            var kpiCards = await multi.ReadFirstOrDefaultAsync<KPICards>();
            var categorySpend = (await multi.ReadAsync<CategorySpend>()).ToList();
            var managerStats = (await multi.ReadAsync<ManagerApproveRejectBarChart>()).ToList();

            var model = new SummaryDashboardViewModel
            {
                KPICards = kpiCards,
                CategorySpend = categorySpend,
                ManagerApproveRejectBarChart = managerStats
            };
            connection.Close();
            return model;
        }
    }
}
