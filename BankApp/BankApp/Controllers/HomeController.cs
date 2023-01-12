﻿using BankApp.Data;
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
        private readonly IMiNIApiCaller _MiNIClient;

        public HomeController(ILogger<HomeController> logger, IClientRepository clientRepository, UserManager<ClientModel> userManager,
            INotRegisteredInquiryRepository notRegisteredInquiryRepository, IEmailSender emailSender, ILoggedInquiryRepository loggedInquiryRepository, 
             IOffersSummaryRepository offersSummaryRepository, IOfferRepository offerRepository, IMiNIApiCaller MiNIClient)
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
            DateTime dt = DateTime.UtcNow;
            inquiry.SubmisionDate = dt.ToString("o");
            int inqIdInOurDb = _loggedInquiryRepository.Add(inquiry);
            
            var inquiryJson = new jsonclass.Loan
            {
                value = (int)inquiry.LoanValue,
                installmentsNumber = inquiry.InstallmentsCount,
                personalData = new jsonclass.PersonalData
                {
                    firstName = user.UserFirstName,
                    lastName = user.UserLastName,
                    birthDate = DateTimeOffset.Parse(user.UserBirthDay).UtcDateTime.ToString("o"),
                    // birthDate = inquiry.UserBirthDay,
                },
                governmentDocument = new jsonclass.GovernmentDocument
                {
                    typeId = BankApp.Models.DocumentTypes.DocumentTypesDictionary[user.ClientGovernmentIDType],
                    name = user.ClientGovernmentIDType,
                    description = user.ClientGovernmentIDType,
                    number = user.ClientGovernmentIDNumber
                },
                jobDetails = new jsonclass.JobDetails
                {
                    typeId = BankApp.Models.JobTypes.JobTypesDictionary[user.ClientJobType],
                    name = user.ClientJobType,
                    description = user.ClientJobType,
                    jobStartDate = DateTimeOffset.Parse(user.ClientJobStartDay).UtcDateTime.ToString("o"),
                    jobEndDate = DateTimeOffset.Parse(user.ClientJobEndDay).UtcDateTime.ToString("o"),
                },
            };
          
            var responseContent = await _MiNIClient.PostInquiryAsync(inquiryJson);
            var inquiryId = JObject.Parse(responseContent)["inquireId"].ToObject<int>();

            await _emailSender.SendEmailAsync(user.Email, "Confirmation of submitting inquiry",
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

            return View("OfferList", new InquiryString { inquiryId = inquiryId, inquiryIdInOurDb = inqIdInOurDb });
        }
        //public async Task<IActionResult> SendInquiry(InquiryModel inquiry)
        //{

        //}
        [HttpPost]
        public async Task<ViewResult> InquiryNotRegistered(NotRegisteredInquiryModel inquiry)
        {
            DateTime dt = DateTime.UtcNow;
            inquiry.SubmissionDate = dt.ToString("o");
            inquiry.ClientJobEndDay = dt.ToString("o");
            inquiry.UserBirthDay = DateTimeOffset.Parse(inquiry.UserBirthDay).UtcDateTime.ToString("o");
            inquiry.ClientJobStartDay = DateTimeOffset.Parse(inquiry.ClientJobStartDay).UtcDateTime.ToString("o");

            int inqIdInOurDb = _notRegisteredInquiryRepository.Add(inquiry);

           
            var inquiryJson = new jsonclass.Loan
            {
                value = (int)inquiry.LoanValue,
                installmentsNumber = inquiry.InstallmentsCount,
                personalData = new jsonclass.PersonalData
                {
                    firstName = inquiry.UserFirstName,
                    lastName = inquiry.UserLastName,
                    birthDate = inquiry.UserBirthDay,
                },
                governmentDocument = new jsonclass.GovernmentDocument
                {
                    typeId = BankApp.Models.DocumentTypes.DocumentTypesDictionary[inquiry.ClientGovernmentIDType],
                    name = inquiry.ClientGovernmentIDType,
                    description = inquiry.ClientGovernmentIDType,
                    number = inquiry.ClientGovernmentIDNumber
                },
                jobDetails = new jsonclass.JobDetails
                {
                    typeId = BankApp.Models.JobTypes.JobTypesDictionary[inquiry.ClientJobType],
                    name = inquiry.ClientJobType,
                    description = inquiry.ClientJobType,
                    jobStartDate = inquiry.ClientJobStartDay,
                    jobEndDate = inquiry.ClientJobEndDay,
                },
            };

            var responseContent = await _MiNIClient.PostInquiryAsync(inquiryJson);
            var inquiryId = (JObject.Parse(responseContent)["inquireId"]).ToObject<int>();

            await _emailSender.SendEmailAsync(inquiry.Email, "Confirmation of submitting inquiry",
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

            return View("OfferList", new InquiryString { inquiryId = inquiryId, inquiryIdInOurDb =  inqIdInOurDb});
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

        [HttpGet]
        public async Task<string?> WaitForOffer(int inquiryId, int inquiryIdInOurDb)
        {
            while (true)
            {
                var inquiryContent = await _MiNIClient.GetInquiryAsync(inquiryId);
                var status = JObject.Parse(inquiryContent)["statusDescription"].ToString();
                if (status == "OfferPrepared") break;
                Thread.Sleep(1000);
            }

            var resultContent = await _MiNIClient.GetInquiryAsync(inquiryId);
            var offerId = JObject.Parse(resultContent)["offerId"].ToObject<int>();

            
            var rOfferContent = await _MiNIClient.GetOfferAsync(offerId);


            var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(rOfferContent);
            //var test = Json(resultContent);
            long offersId = _offerRepository.Add(values);

            bool isNRInquiry = User.Identity.Name is null ? true : false;
            string? clientID = null;
            if(!isNRInquiry)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                clientID = user.Id;
            }
            string bankName = "projectAPI";

            _offersSummaryRepository.Add(inquiryIdInOurDb, isNRInquiry, bankName, offersId, clientID);

            return rOfferContent;

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
        public string Show(string id, string bankName)
        {
            return "/Home/ShowOffer";
        }
        public IActionResult ShowOffer()
        {
            return View();
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
        public IActionResult AcceptOffer(int offerID)
        {
            var r = _offerRepository.UpdateIsOfferAccepted(offerID);
            return View("Index");
        }
    }
}