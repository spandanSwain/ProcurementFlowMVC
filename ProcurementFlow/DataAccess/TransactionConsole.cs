using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProcurementFlow.Data;
using ProcurementFlow.Models.Requisition;
using ProcurementFlow.Models.TranscationConsole;

namespace ProcurementFlow.DataAccess
{
    public class TransactionConsole
    {
        private readonly ProcurementFlowContext _context;
        private readonly IMemoryCache _cache;
        public TransactionConsole(ProcurementFlowContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<TransactionConsoleViewModel> GetAllRequisitionsAsync(string id, int? page)
        {
            //string cacheKey = $"RequisitionTransaction_{id}";
            //if(!_cache.TryGetValue(cacheKey, out List<RequisitionTransactions>? data))
            //{
            //    data = await _context.Set<RequisitionTransactions>()
            //    .FromSqlInterpolated($"EXECUTE SP_SELECT_ALL_REQUISITION_FOR_USER @V_USER={Convert.ToInt32(id)}").AsNoTracking().ToListAsync();
            //    _cache.Set(cacheKey, data, TimeSpan.FromMinutes(5));
            //}
            var data = await _context.Set<RequisitionTransactions>()
                                     .FromSqlInterpolated($"EXECUTE SP_SELECT_ALL_REQUISITION_FOR_USER @V_USER={Convert.ToInt32(id)}")
                                     .AsNoTracking().ToListAsync();
            var model = new TransactionConsoleViewModel
            {
                RequisitionTransactions = PaginatedList<RequisitionTransactions>.CreateFromList(data!, page ?? 1, 5)
            };
            return model;
        }
    }
}
