using BankApp.Data;
using BankApp.Models;
using BankApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientRepository _clientRepository;
        private readonly IList<InquiryModel> _inquiries;


        public HomeController(ILogger<HomeController> logger, IClientRepository clientRepository)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _inquiries = new List<InquiryModel>() {
                new InquiryModel
                {
                    InquiryId = 1,
                    ClientFirstName = "Daniel Lo Nigro",
                    ClientLastName = "Hello ReactJS.NET World!"
                },
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("inquiries")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Comments()
        {
            return Json(_inquiries);
        }

        [Route("inquiries/new")]
        [HttpPost]
        public ActionResult AddInquires(InquiryModel inquiry)
        {
            inquiry.InquiryId = _inquiries.Count + 1;
            
            _inquiries.Add(inquiry);
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            InquiryModel inq1 = new InquiryModel();
            inq1.InquiryId = _inquiries.Count + 1;
            inq1.ClientFirstName = collection[key: "ClientFirstName"];
            inq1.ClientLastName = collection[key: "ClientLastName"];
            // Create a fake ID for this comment
            _inquiries.Add(inq1);

            return Content("Success :)");
        }
    }
}