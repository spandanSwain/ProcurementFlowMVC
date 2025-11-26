using Microsoft.AspNetCore.Mvc;

namespace ProcurementFlow.Controllers
{
    [Route("Procurement")]
    public class ItemReceiptController : Controller
    {
        [Route("ItemReceipt")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Name") == null || HttpContext.Session.GetString("Redirect") != "Procurement") return RedirectToAction("Index", "Login");
            
            return View();
        }
    }
}
