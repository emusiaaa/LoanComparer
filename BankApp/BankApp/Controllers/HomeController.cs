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
using Humanizer.Localisation.TimeToClockNotation;

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
        private readonly System.Net.Http.IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IClientRepository clientRepository, UserManager<ClientModel> userManager,
            INotRegisteredInquiryRepository notRegisteredInquiryRepository, IEmailSender emailSender, ILoggedInquiryRepository loggedInquiryRepository, 
            IHttpClientFactory httpClientFactory, IOffersSummaryRepository offersSummaryRepository, IOfferRepository offerRepository)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _loggedInquiryRepository = loggedInquiryRepository;
            _userManager = userManager;
            _notRegisteredInquiryRepository = notRegisteredInquiryRepository;
            _emailSender = emailSender;
            _clientFactory = httpClientFactory;
            _offersSummaryRepository = offersSummaryRepository;
            _offerRepository = offerRepository;
        }
        public async Task<HttpClient> GetToken()
        {
            var client = _clientFactory.CreateClient("API");
            var clientId = "team4c";
            var clientSecret = "7D84D860-87AC-46AE-B955-68DC7D8C48E3";

            var p = new List<KeyValuePair<string, string>>();
            p.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            p.Add(new KeyValuePair<string, string>("client_id", HttpUtility.UrlEncode(clientId)));
            p.Add(new KeyValuePair<string, string>("client_secret", HttpUtility.UrlEncode(clientSecret)));

            var request = new HttpRequestMessage(HttpMethod.Post, "https://indentitymanager.snet.com.pl/connect/token");
            request.Content = new FormUrlEncodedContent(p);
            request.Headers.Clear();
            
            HttpResponseMessage response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            // response would be a JSON, just extract token from it
            var accessToken = (string)JToken.Parse(responseBody)["access_token"];
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
            return client;
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
        public async Task<IActionResult> HistoryOfInquiries()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = _loggedInquiryRepository.GetAll(user.Id);
            return View(model);
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
            
            HttpClient api = await GetToken();
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
            var stringInquiry = JsonConvert.SerializeObject(inquiryJson);
            var httpContent = new StringContent(stringInquiry, Encoding.UTF8, "application/json");
            var httpResponse = await api.PostAsync("/api/v1/Inquire", httpContent);
            httpResponse.EnsureSuccessStatusCode();

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
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

            HttpClient api = await GetToken();
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
            var stringInquiry = JsonConvert.SerializeObject(inquiryJson);
            var httpContent = new StringContent(stringInquiry, Encoding.UTF8, "application/json");
            var httpResponse = await api.PostAsync("/api/v1/Inquire", httpContent);
            httpResponse.EnsureSuccessStatusCode();

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
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
            HttpClient api = await GetToken();
            while (true)
            {
                var r = await api.GetAsync("/api/v1/Inquire" + $"/{inquiryId}");
                var rContent = await r.Content.ReadAsStringAsync();
                var status = JObject.Parse(rContent)["statusDescription"].ToString();
                if (status == "OfferPrepared") break;
                Thread.Sleep(1000);
            }

            var result = await api.GetAsync("/api/v1/Inquire" + $"/{inquiryId}");
            var resultContent = await result.Content.ReadAsStringAsync();

            var offerId = JObject.Parse(resultContent)["offerId"]?.ToObject<int>();

            var resultOffer = await api.GetAsync("/api/v1/Offer" + $"/{offerId}");
            var rOfferContent = await resultOffer.Content.ReadAsStringAsync();


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
    }
}