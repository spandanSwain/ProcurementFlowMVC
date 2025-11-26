using Dapper;
using ProcurementFlow.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using ProcurementFlow.Models.Common_Pages;
using ProcurementFlow.Models.Item_DropDown;
using System.Data;
using ProcurementFlow.Models.Requisition;
using ProcurementFlow.Models.CatalogItems;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProcurementFlow.DataAccess
{
    public class Items
    {
        private readonly ProcurementFlowContext _context;
        private readonly IMemoryCache _cache;
        public Items(ProcurementFlowContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        private async Task<(List<SelectListItem> LineType, List<SelectListItem> Categories, List<SelectListItem> UOM, List<NonCatalogDisplayItems> nonCatalogDisplayItems)> GetNonCatalogDisplayItemsAsync()
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                var multi = await connection.QueryMultipleAsync(
                    "SP_SELECT_ITEM_DDLS",
                    commandType: CommandType.StoredProcedure
                );

                var category = (await multi.ReadAsync<Categories>()).ToList();
                var lineType = (await multi.ReadAsync<LineTypes>()).ToList();
                var uom = (await multi.ReadAsync<UOM>()).ToList();
                var nonCatalogItems = (await multi.ReadAsync<NonCatalogDisplayItems>()).ToList();

                var lineTypeItems = lineType.Select(r => new SelectListItem { Text = r.LineTypeName, Value = r.LineTypeId.ToString() }).ToList();
                var categoryItems = category.Select(r => new SelectListItem { Text = r.CategoryName, Value = r.CategoryId.ToString() }).ToList();
                var uomItems = uom.Select(r => new SelectListItem { Text = r.UOMName, Value = r.Id.ToString() }).ToList();

                return (lineTypeItems, categoryItems, uomItems, nonCatalogItems)!;
            }            
        }

        public async Task<ItemViewModel> BuildItemViewModelAsync(ItemViewModel? model = null)
        {
            var data = await GetNonCatalogDisplayItemsAsync();
            var cacheModel = new ItemViewModel
            {
                LineType = data.LineType,
                UOM = data.UOM,
                Categories = data.Categories,
                NonCatalogItems = data.nonCatalogDisplayItems
            };
            return cacheModel;
        }

        public async Task<int> InsertNonCatalogItemsAsync(ItemViewModel model)
        {
            try
            {
                var data = await _context.Database
                    .ExecuteSqlInterpolatedAsync
                    ($"SP_INSERT_NON_CATALOG_ITEMS @V_PRICE={model.Price}, @V_UOM={Convert.ToInt32(model.SelectedUOM)}, @V_CATEGORY={Convert.ToInt32(model.SelectedCategory)}, @V_LINE_TYPE={Convert.ToInt32(model.SelectedLineType)}, @V_ITEM_NAME={model.ItemName}, @V_ITEM_TYPE={model.ItemType}, @V_ITEM_DESC={model.ItemDescription}, @V_UPDATEDBY={Convert.ToInt32(model.UpdatedBy)}, @V_UPDATEDON={model.UpdatedOn}");
                return 1;
            }
            catch (Exception ex) { Console.WriteLine($"Exception at DataAccess/Items.cs/InsertNonCatalogItemsAsync() -> {ex}"); return 0; }
        }

        public async Task<CatalogViewModel> GetCatalogItemDetailsAsync(int? page)
        {
            string cacheKey = "catalogItems";
            if(!_cache.TryGetValue(cacheKey, out List<CatalogItems>? data))
            {
                data = await _context.Set<CatalogItems>().FromSqlInterpolated($"EXECUTE SP_SELECT_CATALOG_ITEMS").AsNoTracking().ToListAsync();
                _cache.Set(cacheKey, data, TimeSpan.FromHours(1));
            }

            var model = new CatalogViewModel
            {
                CatalogItems = PaginatedList<CatalogItems>.CreateFromList(data!, page ?? 1, 10)
            };
            return model;
        }

        private async Task<List<SelectListItem>> GetLineTypeAsync()
        {
            var data = await _context.Set<LineTypes>().FromSqlInterpolated($"SP_SELECT_LINE_TYPES").AsNoTracking().ToListAsync();

            var selectedLineType = data.Select(r => new SelectListItem
            {
                Text = r.LineTypeName,
                Value = r.LineTypeId.ToString()
            }).ToList();
            return selectedLineType;
        }
        private async Task<List<SelectListItem>> GetCategoriesAsync()
        {
            var data = await _context.Set<Categories>().FromSqlInterpolated($"SP_SELECT_CATEGORIES").AsNoTracking().ToListAsync();

            var selectedCatrgories = data.Select(r => new SelectListItem
            {
                Text = r.CategoryName,
                Value = r.CategoryId.ToString()
            }).ToList();
            return selectedCatrgories;
        }
        private async Task<List<SelectListItem>> GetUOMAsync()
        {
            var data = await _context.Set<UOM>().FromSqlInterpolated($"SP_SELECT_UOM").AsNoTracking().ToListAsync();

            var selectedUOM = data.Select(r => new SelectListItem
            {
                Text = r.UOMName,
                Value = r.Id.ToString()
            }).ToList();
            return selectedUOM;
        }
    }
}
