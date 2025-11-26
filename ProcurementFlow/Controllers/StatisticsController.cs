using Microsoft.AspNetCore.Mvc;
using ProcurementFlow.DataAccess;

namespace ProcurementFlow.Controllers
{
    [Route("Procurement")]
    public class StatisticsController : Controller
    {
        private readonly Statistics _context;
        public StatisticsController(Statistics context) { _context = context; }

        [HttpGet]
        [Route("Statistics")]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Name") == null || HttpContext.Session.GetString("Redirect") != "Procurement") return RedirectToAction("Index", "Login");

            var model = await _context.GetStatisticsAsync();
            return View(model);
        }
    }
}
