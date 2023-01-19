using BankApp.Client;
using BankApp.Models;
using BankApp.Repositories;
using BankApp.Services;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [Authorize]
    public class LoggedController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILoggedInquiryRepository _loggedInquiryRepository;
        private readonly IOffersSummaryRepository _offersSummaryRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly UserManager<ClientModel> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IOfferServer _offerServer;
        private readonly IInquiryServer _inquiryServer;
        private readonly MiNIApiCaller _MiNIClient;
        private readonly BestAPIApiCaller _BestAPIClient;
        private readonly StrangerApiCaller _StrangerClient;

        public LoggedController(IClientRepository clientRepository, UserManager<ClientModel> userManager,
             IEmailSender emailSender, ILoggedInquiryRepository loggedInquiryRepository,
             IOffersSummaryRepository offersSummaryRepository, IOfferRepository offerRepository, 
             IOfferServer offerServer, IInquiryServer inquiryServer,
             MiNIApiCaller MiNIClient, BestAPIApiCaller bestAPIClient, StrangerApiCaller strangerClient)
        {
            _clientRepository = clientRepository;
            _loggedInquiryRepository = loggedInquiryRepository;
            _userManager = userManager;
            _emailSender = emailSender;
            _offersSummaryRepository = offersSummaryRepository;
            _offerRepository = offerRepository;
            _MiNIClient = MiNIClient;
            _offerServer = offerServer;
            _inquiryServer = inquiryServer;
            _BestAPIClient = bestAPIClient;
            _StrangerClient = strangerClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> HistoryOfInquiries(int pageNumber = 1, int pageSize = 15)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = _loggedInquiryRepository.GetAll(user.Id);
            var result = new PagedResult<InquiryModel>
            {
                Data = model.Skip(excludeRecords).Take(pageSize).ToList(),
                TotalItems = model.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }
        public IActionResult InquiryCompleted()
        {
            return View();
        }
        public IActionResult Inquiry()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Inquiry(InquiryModel inquiry)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var er = errors.Count();
            if (er > 1)
            {
                return View();
            }

            var inquiryJson = _inquiryServer.CreateInquiry(inquiry, user);
            int inqIdInOurDb = _loggedInquiryRepository.Add(inquiry);

            _offerServer.SaveOfferForLogged(_BestAPIClient, inquiryJson, inqIdInOurDb, "BestWorldAPI", user);
            _offerServer.SaveOfferForLogged(_MiNIClient, inquiryJson, inqIdInOurDb, "projectAPI", user);
            _offerServer.SaveOfferForLogged(_StrangerClient, inquiryJson, inqIdInOurDb, "StrangerAPI", user);

            _ = _emailSender.SendEmailAsync(user.Email, "Confirmation of submitting inquiry",
                MailCreator.ConfirmationOfSubmittingAnInquiry(user, inquiry));

            return RedirectToActionPermanent("SuccessConfirmation", "Logged", new { inquiryId = inqIdInOurDb });
        }
        public IActionResult SuccessConfirmation(int inquiryId)
        {
            return View(new InquiryString() { inquiryId = inquiryId });
        }
        public IActionResult OfferList(int inquiryID)
        {
            var model = _offerRepository.GetAllOffersForAGivenInquiry(inquiryID);
            return View(model);
        }
        public async Task<IActionResult> OfferDetails(int offerId, int offerIdInBank)
        {
            var bankName = _offerRepository.GetOfferBank(offerId);
            var offerDetails = _offerRepository.GetAllOffersForAClientForAGivenInquiryForAGivenBank(offerIdInBank, bankName);

            switch (bankName)
            {
                case "projectAPI":
                    ViewBag.document = await _MiNIClient.GetOfferDetailsAsync(offerDetails.DocumentLink); ;
                    break;
                case "BestWorldAPI":
                    ViewBag.document = await _BestAPIClient.GetOfferDetailsAsync(offerDetails.DocumentLink);
                    break;
                case "StrangerAPI":
                    ViewBag.document = await _StrangerClient.GetOfferDetailsAsync(offerDetails.DocumentLink);
                    break;
            }
            return View("OfferDetails", offerDetails);
        }

        [HttpPost]
        [Route("Logged/SendFile/{offerId:int}/{offerIdInBank:int}")]
        public async Task<IActionResult> SendFile(int offerId, int offerIdInBank, IFormFile formFile)
        {
            bool response = false;
            var bankName = _offerRepository.GetOfferBank(offerId);
            switch (bankName)
            {
                case "projectAPI":
                    response = await _MiNIClient.SendFileAsync(offerIdInBank, formFile);
                    break;
                case "BestWorldAPI":
                    response = await _BestAPIClient.SendFileAsync(offerIdInBank, formFile);
                    break;
                case "StrangerAPI":
                    response = await _StrangerClient.SendFileAsync(offerIdInBank, formFile);
                    break;
            }
            if (response) _offerRepository.UpdateIsOfferAccepted(offerId);
            return response ? Ok() : View();
        }
    }
}
