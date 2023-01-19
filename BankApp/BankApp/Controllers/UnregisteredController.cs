using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    public class UnregisteredController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
