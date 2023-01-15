using BankApp.Data;
using BankApp.Models;
using BankApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Dynamic;
using Microsoft.AspNetCore.Identity.UI.Services;
using BankApp.Services;
using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Text.Json;
using NuGet.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using NuGet.Protocol;
using BankApp.Client;
using Humanizer.Localisation.TimeToClockNotation;
using cloudscribe.Pagination.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientRepository _clientRepository;
        private readonly ILoggedInquiryRepository _loggedInquiryRepository;
        private readonly INotRegisteredInquiryRepository _notRegisteredInquiryRepository;
        private readonly IOffersSummaryRepository _offersSummaryRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly UserManager<ClientModel> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IOfferServer _offerServer;
        private readonly InquiryServer _inquiryServer;
        private readonly MiNIApiCaller _MiNIClient;
        private readonly BestAPIApiCaller _BestAPIClient;


        public HomeController(ILogger<HomeController> logger, IClientRepository clientRepository, UserManager<ClientModel> userManager,
            INotRegisteredInquiryRepository notRegisteredInquiryRepository, IEmailSender emailSender, ILoggedInquiryRepository loggedInquiryRepository, 
             IOffersSummaryRepository offersSummaryRepository, IOfferRepository offerRepository, IOfferServer offerServer, InquiryServer inquiryServer, 
             MiNIApiCaller MiNIClient, BestAPIApiCaller bestAPIClient)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _loggedInquiryRepository = loggedInquiryRepository;
            _userManager = userManager;
            _notRegisteredInquiryRepository = notRegisteredInquiryRepository;
            _emailSender = emailSender;
            _offersSummaryRepository = offersSummaryRepository;
            _offerRepository = offerRepository;
            _MiNIClient = MiNIClient;
            _offerServer = offerServer;
            _inquiryServer = inquiryServer;
            _BestAPIClient = bestAPIClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            // var list = _offersSummaryRepository.GetAllOffersForAClientAllInquiries("1957dec4-3d3b-4a83-84b7-3ddeadbe2d06");
            return View();
        }
        [Authorize]
        public async Task<IActionResult> HistoryOfInquiries(int pageNumber=1, int pageSize=15)
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
        [Authorize]
        public async Task<IActionResult> AllBankOffersRequests()
        {
            return View();
        }

        [Authorize]
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

            return Json(model) ;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult InquiryCompleted()
        {
            return View("InquiryCompleted");
        }
        public IActionResult LoggedInquiry()
        {
            return View();
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> LoggedInquiry(InquiryModel inquiry)
        {           
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            inquiry.ClientId = user.Id;
            //DateTime dt = DateTime.UtcNow;
            //inquiry.SubmisionDate = dt.ToString("o");
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var er = errors.Count();
            //if (!ModelState.IsValid)
            if (er>1)
            {
                return View();
            }
            int inqIdInOurDb = _loggedInquiryRepository.Add(inquiry);

            var inquiryJson = _inquiryServer.CreateInquiry(inquiry, user);

            _ = _emailSender.SendEmailAsync(user.Email, "Confirmation of submitting inquiry",
                "<h3>Thanks for submitting your form!</h3>" +
                "<p>Here's a little summary: " +
                "</p><p>Loan value: " + inquiry.LoanValue +
                "</p>" +
                "<p>Number of installments: " + inquiry.InstallmentsCount +
                "</p><p>Name: " + user.UserFirstName + " " + user.UserLastName +
                "</p><p>Government ID Type: " + user.ClientGovernmentIDType +
                "</p><p>Government ID Number: " + user.ClientGovernmentIDNumber +
                "</p><p>Job type: " + user.ClientJobType +
                "</p><p>Income level: " + user.ClientIncomeLevel +
                "</p>");

            _offerServer.SaveOfferForLogged(_BestAPIClient, inquiryJson, inqIdInOurDb, "BestWorldAPI", user);
            _offerServer.SaveOfferForLogged(_MiNIClient, inquiryJson, inqIdInOurDb, "projectAPI", user);

            //return View("OfferList", new InquiryString { inquiryId = inqIdInOurDb });
            return RedirectToActionPermanent("HistoryOfInquiries");
        }
        //public async Task<IActionResult> SendInquiry(InquiryModel inquiry)
        //{

        //}
        [HttpPost]
        public async Task<IActionResult> InquiryNotRegistered(NotRegisteredInquiryModel inquiry)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var er = errors.Count();
            if (!ModelState.IsValid)
            {
                return View();
            }
            DateTime dt = DateTime.UtcNow;
            inquiry.SubmissionDate = dt.ToString("o");
            inquiry.ClientJobEndDay = dt.ToString("o");
            inquiry.UserBirthDay = DateTimeOffset.Parse(inquiry.UserBirthDay).UtcDateTime.ToString("o");
            inquiry.ClientJobStartDay = DateTimeOffset.Parse(inquiry.ClientJobStartDay).UtcDateTime.ToString("o");

            var inquiryJson = _inquiryServer.CreateNRInquiry(inquiry);
            int inqIdInOurDb = _notRegisteredInquiryRepository.Add(inquiry);

            var responseContent = await _MiNIClient.PostInquiryAsync(inquiryJson);
            var inquiryId = (JObject.Parse(responseContent)["inquireId"]).ToObject<int>();

            _ = _emailSender.SendEmailAsync(inquiry.Email, "Confirmation of submitting inquiry",
                             "<h3>Thanks for submitting your form!</h3>" +
                             "<p>Here's a little summary: " +
                             "</p><p>Loan value: " + inquiry.LoanValue +
                             "</p>" +
                             "<p>Number of installments: " + inquiry.InstallmentsCount +
                             "</p><p>Name: " + inquiry.UserFirstName + " " + inquiry.UserLastName +
                             "</p><p>Government ID Type: " + inquiry.ClientGovernmentIDType +
                             "</p><p>Government ID Number: " + inquiry.ClientGovernmentIDNumber +
                             "</p><p>Job type: " + inquiry.ClientJobType +
                             "</p><p>Income level: " + inquiry.ClientIncomeLevel +
                             "</p><p>Link to check the status of  inquiry: <a href=\"https://localhost:7280/Home/OfferList2?inquiryID=" + inquiry.Id+ "&isNR=true\">"  + "here</a>"+
                             "</p>");

            _offerServer.SaveOfferForNotLogged(_BestAPIClient, inquiryJson, inqIdInOurDb, "BestWorldAPI");
            _offerServer.SaveOfferForNotLogged(_MiNIClient, inquiryJson, inqIdInOurDb, "projectAPI");
            //return RedirectToActionPermanent("OfferList", new InquiryString { inquiryId = inqIdInOurDb });
            return RedirectToActionPermanent("HistoryOfInquiries");
        }

        public IActionResult OfferList(InquiryString model)
        {
            return View(model);
        }
        public IActionResult InquiryNotRegistered()
        {
            return View(new NotRegisteredInquiryModel());
        }

        [HttpGet]
        public async Task<string?> BestAPIWaitForOffer()
        {
            return MockOffers.GenerateMockOffer();
        }

        
        public string GetLink(long id, string bankName)
        {
            return "/Home/OfferDetails?id="+ id.ToString() + "&bankName=" + bankName;
        }

        public async Task<IActionResult> OfferDetails(string id, string bankName)
        {
            var offerDetails = _offerRepository.GetAllOffersForAClientForAGivenInquiryForAGivenBank(Int32.Parse(id), bankName);
            //offerDetails.document = await _MiNIClient.GetOfferDetailsAsync(offerDetails.offerModel.DocumentLink);
            ViewBag.document = await _MiNIClient.GetOfferDetailsAsync(offerDetails.DocumentLink);
            return View("OfferDetails", offerDetails);
        }
        public string Show(long id, string bankName)
        {
            return "/Home/ShowOffer?id="+ id.ToString() + "&bankName=" + bankName;
        }
        public IActionResult ShowOffer(long id, string bankName)
        {
            var bankId = _offerRepository.GetOfferIdInBank(id);
            return View(new FileForOfferModel(){ offerId = bankId });
        }
        [HttpPost]
        [Route("Home/SendFile/{offerId:int}/{offerIdInBank:int}")]
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
            }
            return response ? Ok():View();
        }
        public async Task<IActionResult> AcceptDeclineOffer(string id)
        {
            var offer = _offerRepository.GetOfferForBankEmployee(Int32.Parse(id));
            return View(offer);
        }
        public string RedirectToAcceptDeclineOffer(string offerId)
        {
            return "/Home/AcceptDeclineOffer?id=" + offerId.ToString();
        }
        [Authorize]
        public IActionResult MakeDecision(int offerID, bool decision, string employeeID)
        {
            _offerRepository.UpdateIsApprovedByEmployee(offerID, decision, employeeID);
            _MiNIClient.CompleteOfferAsync(offerID);
            return View("AllBankOffersRequests");
        }
        public IActionResult OfferList2(int inquiryID, bool isNR)
        {           
            if (isNR)
            {
                var model = _offerRepository.GetAllOffersForAGivenNRInquiry(inquiryID);
                return View(model);
            }
            else
            {
                var model = _offerRepository.GetAllOffersForAGivenInquiry(inquiryID);
                return View(model);
            }                       
        }
        public string GetBankName(int offerID, int offerIDinBank)
        {
            var bankName = _offerRepository.GetOfferBank(offerID);
            return "/Home/OfferDetails?id=" + offerIDinBank.ToString() + "&bankName=" + bankName;
        }

        [Route("Home/AcceptOffer/{offerId:int}")]
        public IActionResult AcceptOffer(int offerId)
        {
            var r = _offerRepository.UpdateIsOfferAccepted(offerId);
            return RedirectToAction("Index");
        }
    }
}