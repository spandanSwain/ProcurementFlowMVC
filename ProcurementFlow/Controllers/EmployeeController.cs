using Microsoft.AspNetCore.Mvc;
using ProcurementFlow.DataAccess;

namespace ProcurementFlow.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly TransactionConsole _transactionContext;
        public EmployeeController(TransactionConsole transactionContext)
        {
            _transactionContext = transactionContext;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Name") == null) return RedirectToAction("Index", "Login");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        [Route("/Employee/TransactionConsole")]
        [Route("/Procurement/TransactionConsole")]
        public async Task<IActionResult> TransactionConsole(int? page)
        {
            var model = await _transactionContext.GetAllRequisitionsAsync(HttpContext.Session.GetString("UserID")!, page);
            return View(model);
        }
    }
}