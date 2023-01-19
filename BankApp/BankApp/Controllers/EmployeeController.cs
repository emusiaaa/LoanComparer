using BankApp.Client;
using BankApp.Models;
using BankApp.Repositories;
using BankApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        private readonly UserManager<ClientModel> _userManager;
        private readonly IOfferRepository _offerRepository;
        public EmployeeController(IOfferRepository offerRepository, UserManager<ClientModel> userManager)
        {
            _userManager = userManager;
            _offerRepository = offerRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AllBankOffersRequests()
        {
            return View();
        }
        public async Task<IActionResult> FilterOffersRequests(string searchString, int dateRange)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.IsBankEmployee)
            {
                var model = _offerRepository.GetAllOffersForBankEmployee(user.Id, user.EmployeesBankName);
                return Json(model);
            }

            return null;
        }
    }
}
