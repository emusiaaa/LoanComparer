using BankApp.Client;
using BankApp.Models;
using BankApp.Repositories;
using BankApp.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    public class UnregisteredController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly INotRegisteredInquiryRepository _notRegisteredInquiryRepository;
        private readonly IOffersSummaryRepository _offersSummaryRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IEmailSender _emailSender;
        private readonly IOfferServer _offerServer;
        private readonly IInquiryServer _inquiryServer;
        private readonly MiNIApiCaller _MiNIClient;
        private readonly BestAPIApiCaller _BestAPIClient;
        private readonly StrangerApiCaller _StrangerClient;

        public UnregisteredController(IClientRepository clientRepository,INotRegisteredInquiryRepository notRegisteredInquiryRepository,
             IEmailSender emailSender, IOfferServer offerServer, IInquiryServer inquiryServer,
             IOffersSummaryRepository offersSummaryRepository, IOfferRepository offerRepository,
             MiNIApiCaller MiNIClient, BestAPIApiCaller bestAPIClient, StrangerApiCaller strangerClient)
        {
            _clientRepository = clientRepository;
            _notRegisteredInquiryRepository = notRegisteredInquiryRepository;
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

        public IActionResult Inquiry()
        {
            return View(new NotRegisteredInquiryModel());
        }

        [HttpPost]
        public async Task<IActionResult> Inquiry(NotRegisteredInquiryModel inquiry)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var er = errors.Count();
            if (!ModelState.IsValid)
            {
                return View();
            }

            var inquiryJson = _inquiryServer.CreateNRInquiry(inquiry);
            int inqIdInOurDb = _notRegisteredInquiryRepository.Add(inquiry);

            _offerServer.SaveOfferForNotLogged(_BestAPIClient, inquiryJson, inqIdInOurDb, "BestWorldAPI");
            _offerServer.SaveOfferForNotLogged(_MiNIClient, inquiryJson, inqIdInOurDb, "projectAPI");
            _offerServer.SaveOfferForNotLogged(_StrangerClient, inquiryJson, inqIdInOurDb, "StrangerAPI");

            _ = _emailSender.SendEmailAsync(inquiry.Email, "Confirmation of submitting inquiry",
                             MailCreator.ConfirmationOfSubmittingAnInquiryNR(inquiry));

            return RedirectToActionPermanent("SuccessConfirmation", "Unregistered", new { inquiryId = inqIdInOurDb });
        }
        public IActionResult SuccessConfirmation(int inquiryId)
        {
            return View(new InquiryString() { inquiryId = inquiryId });
        }
        public IActionResult OfferList(int inquiryID)
        {
            var model = _offerRepository.GetAllOffersForAGivenNRInquiry(inquiryID);
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
        [Route("Unregistered/SendFile/{offerId:int}/{offerIdInBank:int}")]
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
