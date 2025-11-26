using Dapper;
using ProcurementFlow.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using ProcurementFlow.Models.Requisition;
using System.Data;
using System.Runtime.InteropServices;

namespace ProcurementFlow.DataAccess
{
    public class Requisition
    {
        private readonly ProcurementFlowContext _context;
        private readonly IMemoryCache _cache;
        public Requisition(ProcurementFlowContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }        

        public async Task<(List<SelectListItem> Items, RequisitionOverview Summary, List<RequisitionListItems> RequisitionListItems)> LoadItemsAsync(string userID)
        {
            using var connection = _context.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            var multi = await connection.QueryMultipleAsync(
                "SP_SELECT_ITEMS",
                new { V_USER = Convert.ToInt32(userID) },
                commandType: CommandType.StoredProcedure
            );

            var itemsRaw = (await multi.ReadAsync<ItemDDL>()).ToList();
            var summary = await multi.ReadFirstOrDefaultAsync<RequisitionOverview>();
            var requisitionList = (await multi.ReadAsync<RequisitionListItems>()).ToList();

            var items = itemsRaw.Select(r => new SelectListItem
            {
                Text = r.ItemName,
                Value = r.Id.ToString()
            }).ToList();

            return (items, summary, requisitionList)!;
        }

        public async Task<ItemDetails> LoadItemDetailsAsync(int id)
        {
            var data = await _context.Set<ItemDetails>()
                .FromSqlInterpolated($"EXECUTE SP_SELECT_ITEM_DETAILS @V_ID={id}")
                .AsNoTracking().ToListAsync();
            return data[0];
        }

        public async Task<RequisitionLineItems> LoadRequisitionItemDetailsAsync(int id)
        {
            var data = await _context.Set<RequisitionLineItems>()
                .FromSqlInterpolated($"EXECUTE SP_SELECT_REQUISITION_ITEM_DETAILS @V_ID={id}")
                .AsNoTracking().ToListAsync();
            return data[0];
        }

        public async Task<int> AddRequisitionLineAsync(RequisitionViewModel model)
        {
            try
            {
                var _ = await _context.Database
                       .ExecuteSqlInterpolatedAsync
                    ($"EXECUTE SP_INSERT_REQUISITION_LINE @V_ITEM={model.SelectedItemID}, @V_STATUS={model.Status}, @V_URGENT={Convert.ToInt32(model.Urgent)}, @V_QUANTITY={model.Quantity}, @V_PRICE={model.CalculatedPrice}, @V_UPDATEDON={model.UpdatedOn}, @V_REQUESTOR={Convert.ToInt32(model.Requestor)}, @V_DELIVERY_ADDR={model.DeliveryAddress}, @V_DELIVERY_LOC={model.DeliveryLocation}, @V_REQUESTED_DATE={model.RequestedDate}");
                return 1;
            }
            catch (Exception ex) { Console.WriteLine($"Exception at DataAccess/Requisition.cs/AddRequisitionLineAsync() -> {ex}"); return 0; }
        }

        public async Task<int> UpdateRequisitionLineAsync(RequisitionViewModel model)
        {
            try
            {
                var _ = await _context.Database
                       .ExecuteSqlInterpolatedAsync
                       ($"EXECUTE SP_UPDATE_REQUISITION_LINE @V_ID={model.SelectedRequisitionItemId}, @V_ITEM={model.SelectedItemID}, @V_STATUS={model.Status}, @V_URGENT={Convert.ToInt32(model.Urgent)}, @V_QUANTITY={model.Quantity}, @V_PRICE={model.CalculatedPrice}, @V_UPDATEDON={model.UpdatedOn}, @V_REQUESTOR={Convert.ToInt32(model.Requestor)}, @V_DELIVERY_ADDR={model.DeliveryAddress}, @V_DELIVERY_LOC={model.DeliveryLocation}, @V_REQUESTED_DATE={model.RequestedDate}");
                return 1;
            }
            catch(Exception ex) { Console.WriteLine($"Exception at DataAccess/Requisition.cs/UpdateRequisitionLineAsync() -> {ex}"); return 0; }
        }

        public async Task<int> DeleteRequisitionLineAsync(RequisitionViewModel model)
        {
            try
            {
                var _ = await _context.Database.ExecuteSqlInterpolatedAsync($"EXECUTE SP_DELETE_REQUISITION_LINE @V_ID={model.SelectedRequisitionItemId}");
                return 1;
            }
            catch(Exception ex) { Console.WriteLine($"Exception at DataAccess/Requisition.cs/DeleteRequisitionLineAsync() -> {ex}"); return 0; }
        }
        private async Task<List<SelectListItem>> LoadItemsDropDownAsync()
        {
            const string cacheKey = "ItemsDDL";
            if (!_cache.TryGetValue(cacheKey, out List<SelectListItem>? items))
            {
                var data = await _context.Set<ItemDDL>().FromSqlInterpolated($"EXECUTE SP_SELECT_ITEMS").AsNoTracking().ToListAsync();
                items = data.Select(r => new SelectListItem
                {
                    Text = r.ItemName,
                    Value = r.Id.ToString()
                }).ToList();
                _cache.Set(cacheKey, items, TimeSpan.FromHours(3));
            }
            return items!;
        }
    }
}
