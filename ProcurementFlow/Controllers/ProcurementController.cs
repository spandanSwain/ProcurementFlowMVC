using Microsoft.AspNetCore.Mvc;
using ProcurementFlow.DataAccess;
using ProcurementFlow.Models.Requisition;
using Microsoft.Extensions.Caching.Memory;
using ProcurementFlow.Models.Item_DropDown;
using ProcurementFlow.Models.CatalogItems;
using System.Runtime.CompilerServices;

namespace ProcurementFlow.Controllers
{
    public class ProcurementController : Controller
    {
        private readonly Items _itemcontext;
        private readonly Requisition _requisitioncontext;
        private readonly IMemoryCache _cache;
        public ProcurementController(Items itemcontext, Requisition requisitioncontext, IMemoryCache cache)
        {
            _itemcontext = itemcontext;
            _cache = cache;
            _requisitioncontext = requisitioncontext;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Name") == null) return RedirectToAction("Index", "Login");
            return View();
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index", HttpContext.Session.GetString("Redirect"));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        [Route("/Employee/Requisition")]
        [Route("/Procurement/Requisition")]
        public async Task<IActionResult> Requisition(int? page)
        {
            var (items, summary, requisitions) = await _requisitioncontext.LoadItemsAsync(HttpContext.Session.GetString("UserID")!.ToString());
            var model = new RequisitionViewModel
            {
                Items = items,
                TotalRequisition = summary.Total,
                ApprovedRequisition = summary.Approved,
                RejectedRequisition = summary.Rejected,
                RequisitionListItems = PaginatedList<RequisitionListItems>.CreateFromList(requisitions, page ?? 1, 2)
            };
            return View(model);
        }

        [HttpGet] // Axios api to get details from selected item id
        public async Task<JsonResult> GetItemDetails(int itemId)
        {
            var data = await _requisitioncontext.LoadItemDetailsAsync(itemId);
            return Json(new { success = true, data });
        }
        [HttpGet] // Axios api to get details from selected requisition id
        public async Task<JsonResult> GetRequisitionItemDetails(int lineItemId)
        {
            var data = await _requisitioncontext.LoadRequisitionItemDetailsAsync(lineItemId);
            Console.WriteLine($"data from api = {data}");
            return Json(new { success = true, data });
        }

        [HttpPost]
        [Route("/Employee/Requisition")]
        [Route("/Procurement/Requisition")]
        public async Task<IActionResult> Requisition(RequisitionViewModel model)
        {
            if(model.Action == "edit")
            {
                var updRequisition = await _requisitioncontext.UpdateRequisitionLineAsync(model);
                TempData["RequisitionMessage"] = (updRequisition == 1) ? "Done" : "Unable to add items ... Try again";
                return RedirectToAction("Requisition", "Procurement");
            }
            else if (model.Action == "delete")
            {
                var dltRequisition = await _requisitioncontext.DeleteRequisitionLineAsync(model);
                TempData["RequisitionMessage"] = (dltRequisition == 1) ? "Done" : "Unable to delete item... Try again";
                return RedirectToAction("Requisition", "Procurement");
            }
            var addRequisition = await _requisitioncontext.AddRequisitionLineAsync(model);
            TempData["RequisitionMessage"] = (addRequisition == 1) ? "Done" : "Unable to edit items ... Try again";
            return RedirectToAction("Requisition", "Procurement");
        }

        [HttpGet]
        [Route("/Employee/AddNonCatalog")]
        [Route("/Procurement/AddNonCatalog")]
        public async Task<IActionResult> AddNonCatalog()
        {
            var model = await _itemcontext.BuildItemViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [Route("/Employee/AddNonCatalog")]
        [Route("/Procurement/AddNonCatalog")]
        public async Task<IActionResult> AddNonCatalog(ItemViewModel model)
        {
            var addItems = await _itemcontext.InsertNonCatalogItemsAsync(model);
            TempData["NonCatalogMessage"] = (addItems == 1) ? "Done" : "Unable to add items ... Try again";
            return RedirectToAction("AddNonCatalog", "Procurement");
        }

        [HttpGet]
        [Route("/Employee/CatalogItems")]
        [Route("/Procurement/CatalogItems")]
        public async Task<IActionResult> CatalogItems(int? page)
        {
            var model = await _itemcontext.GetCatalogItemDetailsAsync(page);
            return View(model);
        }
    }
}
