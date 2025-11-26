using Microsoft.AspNetCore.Mvc;
using ProcurementFlow.DataAccess;

namespace ProcurementFlow.Controllers
{
    [Route("Procurement")]
    public class SummaryController : Controller
    {
        private readonly SummaryDashboard _context;
        public SummaryController(SummaryDashboard context) { this._context = context; }

        [HttpGet]
        [Route("SummaryDashboard")]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Name") == null || HttpContext.Session.GetString("Redirect") != "Procurement") return RedirectToAction("Index", "Login");

            var model = await _context.LoadSummaryDashboardAsync(HttpContext.Session.GetString("UserID")!);
            return View(model);
        }
    }
}
