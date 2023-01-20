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
        private readonly ILoggedInquiryRepository _loggedInquiryRepository;
        private readonly INotRegisteredInquiryRepository _notRegisteredInquiryRepository;
        private readonly IEmailSender _emailSender;
        private readonly MiNIApiCaller _MiNIClient;
        private readonly BestAPIApiCaller _BestAPIClient;
        private readonly StrangerApiCaller _StrangerClient;
        public EmployeeController(IOfferRepository offerRepository, UserManager<ClientModel> userManager,
            INotRegisteredInquiryRepository notRegisteredInquiryRepository, ILoggedInquiryRepository loggedInquiryRepository,
            IEmailSender emailSender, MiNIApiCaller MiNIClient, BestAPIApiCaller bestAPIClient, StrangerApiCaller strangerClient)
        {
            _userManager = userManager;
            _offerRepository = offerRepository;
            _loggedInquiryRepository = loggedInquiryRepository;
            _notRegisteredInquiryRepository = notRegisteredInquiryRepository;
            _emailSender = emailSender;
            _emailSender = emailSender;
            _MiNIClient = MiNIClient;
            _BestAPIClient = bestAPIClient;
            _StrangerClient = strangerClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AllBankOffersRequests()
        {
            return View();
        }
        public IActionResult AllBankInquiries()
        {
            return View();
        }
        public async Task<IActionResult> Filter(string searchString, int dateRange)
        {
            IEnumerable<AllInquiryViewModel> model1, model2;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            model1 = _loggedInquiryRepository.ToAllInquiryModel();
            model2 = _notRegisteredInquiryRepository.ToAllInquiryModel();
            var model = model1.Concat(model2);

            return Json(model);
        }
        public async Task<IActionResult> FilterOffersRequests(string searchString, int dateRange)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = _offerRepository.GetAllOffersForBankEmployee(user.Id, user.EmployeesBankName);
            return Json(model);
        }
        public string RedirectToAcceptDeclineOffer(string offerId)
        {
            return "/Employee/AcceptDeclineOffer?id=" + offerId.ToString();
        }
        public async Task<IActionResult> AcceptDeclineOffer(string id)
        {
            //var offer = _offerRepository.GetOfferForBankEmployee(Int32.Parse(id));
            var info = _offerRepository.OfferDetailsForBankEmployeeType(Int32.Parse(id));
            return View(info);
        }
        public async Task<IActionResult> MakeDecision(int offerID, string email, bool decision)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            _offerRepository.UpdateIsApprovedByEmployee(offerID, decision, user.Id);
            if (decision)
            {
                switch (user.EmployeesBankName)
                {
                    case "projectAPI":
                        _MiNIClient.CompleteOfferAsync(offerID);
                        break;
                    case "BestWorldAPI":
                        _BestAPIClient.CompleteOfferAsync(offerID);
                        break;
                    case "StrangerAPI":
                        _StrangerClient.CompleteOfferAsync(offerID);
                        break;
                }
            }
            _ = _emailSender.SendEmailAsync(email, "Bank decision",
                MailCreator.EmployeeDecision(decision));
            return RedirectToAction("AllBankOffersRequests");
        }
    }
}
