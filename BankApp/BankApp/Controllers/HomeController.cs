using BankApp.Data;
using BankApp.Models;
using BankApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.UI.Services;
using BankApp.Services;
using BankApp.Client;

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        //public string Show(long id, string bankName)
        //{
        //    return "/Home/ShowOffer?id=" + id.ToString() + "&bankName=" + bankName;
        //}
        //public IActionResult ShowOffer(long id, string bankName)
        //{
        //    var bankId = _offerRepository.GetOfferIdInBank(id);
        //    return View(new FileForOfferModel() { offerId = bankId });
        //}
    
        //public string[] InquiryIDinOurDb(int offerId)
        //{
        //    var c =_offersSummaryRepository.GetInquiryIdInOurDb(offerId);
        //    //var x = Json(c);
        //    string[] cs = { c.id.ToString(), c.isNR.ToString() };
        //    return cs ;
        //}
    }
}