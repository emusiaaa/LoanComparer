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

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientRepository _clientRepository;
        private readonly INotRegisteredInquiryRepository _notRegisteredInquiryRepository;
        private readonly UserManager<ClientModel> _userManager;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, IClientRepository clientRepository, UserManager<ClientModel> userManager,
            INotRegisteredInquiryRepository notRegisteredInquiryRepository, IEmailSender emailSender)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _userManager = userManager;
            _notRegisteredInquiryRepository = notRegisteredInquiryRepository;
            _emailSender = emailSender;
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

        [Authorize, HttpGet]
        public async Task<IActionResult> Inquiry()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            dynamic mymodel = new ExpandoObject();
            mymodel.Inquiry = new InquiryModel();
            mymodel.Client = user;
            return View(mymodel);
        }

        [HttpPost]
        public async Task<ViewResult> InquiryNotRegistered(NotRegisteredInquiryModel inquiry)
        {
            DateTime dt = DateTime.Now;
            inquiry.SubmissionDate = dt.ToString("yyyy-MM-dd");
            _notRegisteredInquiryRepository.Add(inquiry);
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