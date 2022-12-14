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

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientRepository _clientRepository;
        private readonly ILoggedInquiryRepository _loggedInquiryRepository;
        private readonly INotRegisteredInquiryRepository _notRegisteredInquiryRepository;
        private readonly UserManager<ClientModel> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly System.Net.Http.IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IClientRepository clientRepository, UserManager<ClientModel> userManager,
            INotRegisteredInquiryRepository notRegisteredInquiryRepository, IEmailSender emailSender, ILoggedInquiryRepository loggedInquiryRepository, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _loggedInquiryRepository = loggedInquiryRepository;
            _userManager = userManager;
            _notRegisteredInquiryRepository = notRegisteredInquiryRepository;
            _emailSender = emailSender;
            _clientFactory = httpClientFactory;
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
            
            var inquiryJson = new jsonclass.Loan
            {
                value = 15000,
                installmentsNumber = 20,
                personalData = new jsonclass.PersonalData
                {
                    firstName = "Ac",
                    lastName = "Ba",
                    birthDate = (new DateTime(1970, 8, 15, 13, 45, 30,
                                    DateTimeKind.Utc)).ToString("o"),
                },
                governmentDocument = new jsonclass.GovernmentDocument
                {
                    typeId = 2,
                    name = "Passport",
                    description = "Passport",
                    number = "q1223",
                },
                jobDetails = new jsonclass.JobDetails
                {
                    typeId = 37,
                    name = "Agent",
                    description = "Agent",
                    jobStartDate = "2022-09-16T19:27:33.591Z",
                    jobEndDate = "2022-12-06T19:27:33.591Z",
                },
            };
            var stringInquiry = JsonConvert.SerializeObject(inquiryJson);
            var httpContent = new StringContent(stringInquiry, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync("/api/v1/Inquire", httpContent);
            httpResponse.EnsureSuccessStatusCode();

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var inquireId = JObject.Parse(responseContent)["inquireId"];
            return client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            _ = GetToken();
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
        public async Task<IActionResult> AllBankInquiries(string sortOrder, string searchString, int dateRange)
        {
            ViewData["DateAscSort"] = "";
            ViewData["DateDescSort"] = "date_desc";
            ViewData["LoanAscSort"] = "loan_asc";
            ViewData["LoanDescSort"] = "loan_desc";
            ViewData["CurrentFilter"] = searchString;
            ViewData["DateFilter"] = dateRange;

            IEnumerable<AllInquiryViewModel> model1, model2;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (String.IsNullOrEmpty(searchString) && dateRange==0)
            {
                model1 = _loggedInquiryRepository.ToAllInquiryModel();
                model2 = _notRegisteredInquiryRepository.ToAllInquiryModel();
            }
            else
            {
                model1 = _loggedInquiryRepository.ToAllInquiryModelFilteredByName(searchString, dateRange);
                model2 = _notRegisteredInquiryRepository.ToAllInquiryModelFilteredByName(searchString, dateRange);
            }

            var model = model1.Concat(model2);

            switch (sortOrder)
            {
                case "date_desc":
                    model = model
                        .OrderByDescending(d => d.SubmissionDate)
                        .ToList();
                    break;
                case "loan_desc":
                    model = model
                        .OrderByDescending(d => d.LoanValue)
                        .ToList();
                    break;
                case "loan_asc":
                    model = model
                        .OrderBy(d => d.LoanValue)
                        .ToList();
                    break;
                default:
                    model = model
                        .OrderBy(d => d.SubmissionDate)
                        .ToList();
                    break;
            }
            return View(model);
        }
        public async Task<IActionResult> Filter()
        {
            IEnumerable<AllInquiryViewModel> model1, model2;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            //if (String.IsNullOrEmpty(searchString) && dateRange == 0)
            //{
                model1 = _loggedInquiryRepository.ToAllInquiryModel();
                model2 = _notRegisteredInquiryRepository.ToAllInquiryModel();
            //}
            //else
            //{
            //    model1 = _loggedInquiryRepository.ToAllInquiryModelFilteredByName(searchString, dateRange);
            //    model2 = _notRegisteredInquiryRepository.ToAllInquiryModelFilteredByName(searchString, dateRange);
            //}

            var model = model1.Concat(model2);
            var j = Json(model);
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
            DateTime dt = DateTime.Now;
            inquiry.SubmisionDate = dt.ToString("yyyy-MM-dd");

            _loggedInquiryRepository.Add(inquiry);

            HttpClient api = await GetToken();
            var stringInquiry = JsonConvert.SerializeObject(inquiry);
            var httpContent = new StringContent(stringInquiry, Encoding.UTF8, "application/json");
            var httpResponse = await api.PostAsync("/api/v1/Inquire", httpContent);

            httpResponse.EnsureSuccessStatusCode();
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var inquireId = JObject.Parse(responseContent)["inquireId"];

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

            return View("NotRegisteredInquirySubmitted");
        }

        [HttpPost]
        public async Task<ViewResult> InquiryNotRegistered(NotRegisteredInquiryModel inquiry)
        {
            DateTime dt = DateTime.Now;
            inquiry.SubmissionDate = dt.ToString("yyyy-MM-dd");
            _notRegisteredInquiryRepository.Add(inquiry);

            HttpClient api = await GetToken();
            var inquiryJson = new jsonclass.Loan
            {
                value = 10000,
                installmentsNumber = 40,
                personalData = new jsonclass.PersonalData
                {
                    firstName = "A",
                    lastName = "B",
                    birthDate = (new DateTime(1980, 8, 15, 13, 45, 30,
                                    DateTimeKind.Utc)).ToString("o"),
                },
                governmentDocument = new jsonclass.GovernmentDocument
                {
                    typeId = 2,
                    name = "Passport",
                    description = "Passport",
                    number = "q123",
                },
                jobDetails = new jsonclass.JobDetails
                {
                    typeId = 37,
                    name = "Agent",
                    description = "Agent",
                    jobStartDate = "2022-09-16T19:27:33.591Z",
                    jobEndDate = "2022-12-06T19:27:33.591Z",
                },
            };
            var stringInquiry = JsonConvert.SerializeObject(inquiry);
            var httpContent = new StringContent(stringInquiry, Encoding.UTF8, "application/json");
            var httpResponse = await api.PostAsync("/api/v1/Inquire", httpContent);
            httpResponse.EnsureSuccessStatusCode();
            
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var inquireId = (JObject.Parse(responseContent)["inquireId"]).ToObject<int>();

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
            return View("NotRegisteredInquirySubmitted");
        }

        public IActionResult InquiryNotRegistered()
        {
            return View(new NotRegisteredInquiryModel());
        }
    }
}