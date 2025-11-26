using Microsoft.AspNetCore.Mvc;
using ProcurementFlow.Models.Login;
using ProcurementFlow.DataAccess;
using ProcurementFlow.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace ProcurementFlow.Controllers
{
    public class LoginController : Controller
    {
        private readonly Login _context;
        private readonly IMemoryCache _cache;
        public LoginController(Login context, IMemoryCache cache) {
            _context = context;
            _cache = cache;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login([Bind("UserName, Password")] Users users)
        {
            if (!string.IsNullOrWhiteSpace(users.UserName) || !string.IsNullOrWhiteSpace(users.UserName))
            {
                string? userName = users.UserName!; string password = users.Password!;
                string cacheKey = $"login_{userName}_{password}";
                
                if(_cache.TryGetValue(cacheKey, out (string Name, string Role, string Message, string UserID) cachedUser))
                {
                    HttpContext.Session.SetString("Name", cachedUser.Name); HttpContext.Session.SetString("Role", cachedUser.Role);
                    HttpContext.Session.SetString("User", userName); HttpContext.Session.SetString("Password", password);
                    HttpContext.Session.SetString("UserID", cachedUser.UserID);

                    return RedirectToAction("Index", cachedUser.Message);
                }

                (string? name, string? role, int validationKey, int userID) = await _context.ValidateUsersAsync(userName, password);
                //int validationKey = 1; // for testing
                string message = validationKey switch
                {
                    1 => "Employee",
                    2 => "Procurement",
                    3 => "Invalid Credentials",
                    4 => "No such user found",
                    _ => "Invalid Credentials!"
                };

                if (validationKey == 1 || validationKey == 2)
                {
                    HttpContext.Session.SetString("Name", name!); HttpContext.Session.SetString("Role", role!);
                    HttpContext.Session.SetString("User", userName); HttpContext.Session.SetString("Password", password);
                    HttpContext.Session.SetString("UserID", userID.ToString());
                    HttpContext.Session.SetString("Redirect", message);

                    _cache.Set(cacheKey, (name, role, message, userID), new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                }

                TempData["Message"] = (validationKey != 1 && validationKey != 2) ? message : null;
                return (validationKey == 1 || validationKey == 2) ? RedirectToAction("Index", message) : View("Index");
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
