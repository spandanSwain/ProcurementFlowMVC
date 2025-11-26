using Microsoft.AspNetCore.Mvc;
using ProcurementFlow.DataAccess;
using ProcurementFlow.Models.PurchaseOrder;

namespace ProcurementFlow.Controllers
{
    [Route("Procurement")]
    public class PurchaseOrderController : Controller
    {
        private readonly PurchaseOrder _purchaseContext;
        public PurchaseOrderController(PurchaseOrder purchaseContext)
        {
            _purchaseContext = purchaseContext;
        }

        [HttpGet]
        [Route("PurchaseOrder")]
        public async Task<IActionResult> Index(int? page)
        {
            if (HttpContext.Session.GetString("Name") == null || HttpContext.Session.GetString("Redirect") != "Procurement") return RedirectToAction("Index", "Login");

            var model = await _purchaseContext.GetPendingRequisitionsAsync(page);
            return View(model);
        }

        [HttpPost]
        [Route("PurchaseOrder")]
        public async Task<IActionResult> Index(PurchaseOrderViewModel model)
        {
            if(model.Action == "Approve")
            {
                var approvedData = await _purchaseContext.PerformApprovalActionAsync(model);
                TempData["PurchaseMessage"] = (approvedData == 1) ? "Done" : "Unable to perform action ... Try again";
                return RedirectToAction("Index", "PurchaseOrder");
            }
            var rejectedData = await _purchaseContext.PerformRejectionActionAsync(model);
            TempData["PurchaseMessage"] = (rejectedData == 1) ? "Done" : "Unable to perform action ... Try again";
            return RedirectToAction("Index", "PurchaseOrder");
        }
    }
}