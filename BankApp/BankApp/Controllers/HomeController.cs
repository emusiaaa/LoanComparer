using BankApp.Data;
using BankApp.Models;
using BankApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Dynamic;


namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientRepository _clientRepository;
        private readonly UserManager<ClientModel> _userManager;

        public HomeController(ILogger<HomeController> logger, IClientRepository clientRepository, UserManager<ClientModel> userManager)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _userManager = userManager;
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
        [HttpPost]
        public ViewResult Inquiry(InquiryModel inquiry)
        {
            return View();
        }

        [Authorize, HttpGet("Profile")]
        public async Task<IActionResult> Inquiry()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            dynamic mymodel = new ExpandoObject();
            mymodel.Inquiry = new InquiryModel();
            mymodel.Client = user;
            return View(mymodel);
        }

        [HttpPost]
        public ViewResult InquiryNotRegistered(InquiryModel inquiry)
        {
            return View();
        }

        public IActionResult InquiryNotRegistered()
        {
            return View();
        }

        

        //public IActionResult Inquiry()
        //{
        //    var user = _userManager.FindByIdAsync(User.Identity.Name);

        //    //return Json(new
        //    //{
        //    //    IsAuthenticated = User.Identity.IsAuthenticated,
        //    //    Id = User.Identity.Name,
        //    //    Name = $"{user.UserName} {user.UserLastName}",
        //    //    Type = User.Identity.AuthenticationType,
        //    //});
        //}
    }
}