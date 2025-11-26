using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using ProcurementFlow.Data;
using ProcurementFlow.Models.Login;
using ProcurementFlow.Models.Statistics;
using ProcurementFlow.Models.SummaryDashboards;

namespace ProcurementFlow.DataAccess
{
    public class Statistics
    {
        private readonly ProcurementFlowContext _context;
        public Statistics(ProcurementFlowContext context) { _context = context; }

        public async Task<StatisticsViewModel> GetStatisticsAsync()
        {
            using var connection = _context.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            var multi = await connection.QueryMultipleAsync(
            "SP_ORG_STATISTICS",
                commandType: CommandType.StoredProcedure
            );

            var kpiCards = await multi.ReadFirstOrDefaultAsync<TotalKPIStats>();
            var poPipeline = await multi.ReadFirstOrDefaultAsync<PurchaseOrderPipeline>();
            var monthlyTrend = (await multi.ReadAsync<MonthlyTrend>()).ToList();
            var topManagers = (await multi.ReadAsync<TopManagers>()).ToList();
            var spendPerCategory = (await multi.ReadAsync<SpendPerCategory>()).ToList();
            var topRequisitions = (await multi.ReadAsync<TopRequisitions>()).ToList();

            var model = new StatisticsViewModel
            {
                TotalKPIStats = kpiCards,
                PurchaseOrderPipeline = poPipeline,
                MonthlyTrend = monthlyTrend,
                TopManagers = topManagers,
                SpendPerCategory = spendPerCategory,
                TopRequisitions = topRequisitions,
            };
            connection.Close();

            return model;
        }
    }
}
