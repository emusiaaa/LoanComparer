using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
