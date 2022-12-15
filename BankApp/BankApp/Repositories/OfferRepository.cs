using BankApp.Data;
using BankApp.Models;
using System.Text.Json.Nodes;

namespace BankApp.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly LoansComparerDBContext _context;

        public OfferRepository(LoansComparerDBContext context)
        {
            _context = context;
        }

        public void Add(Dictionary<string, dynamic> jsonOffer)
        {
            OfferModel offer = new OfferModel();

            offer.OfferIdInBank = jsonOffer["id"];
            offer.Percentage = jsonOffer["percentage"];
            offer.MonthlyInstallment = jsonOffer["monthlyInstallment"];
            offer.RequestedValue = jsonOffer["requestedValue"];
            offer.RequestedPeriodInMonth = jsonOffer["requestedPeriodInMonth"];
            offer.StatusId = jsonOffer["statusId"];
            offer.StatusDescription = jsonOffer["statusDescription"];
            offer.InquireId = jsonOffer["inquireId"];
            offer.CreateDate = jsonOffer["createDate"];
            offer.UpdateDate = jsonOffer["updateDate"];
            offer.ApprovedBy = jsonOffer["approvedBy"];
            offer.DocumentLink = jsonOffer["documentLink"];
            offer.DocumentLinkValidDate = jsonOffer["documentLinkValidDate"];

            var res = _context.Offers.Add(offer); // może da się stąd wyciągnąć id stworzonej oferty i zwrócić z Add żeby OffersSummary miało??
            _context.SaveChanges();
        }

        public void Delete(int offersID)
        {
            var result = _context.Offers.SingleOrDefault(x => x.Id == offersID);
            if (result != null)
            {
                _context.Offers.Remove(result);
                _context.SaveChanges();
            }
        }

        public IEnumerable<OfferModel> GetAllOffersForAClientAllInquiries(string clientID)
        {
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where offerSummary.ClientId == clientID
                        select offer;
            return query.ToList();
        }

        public IEnumerable<OfferModel> GetAllOffersForAClientForAGivenInquiry(string clientID, int inquiryID)
        {
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where (offerSummary.ClientId == clientID
                        && offerSummary.InquiryIdInOurDb == inquiryID)
                        select offer;
            return query.ToList();
        }

        public IEnumerable<OfferModel> GetAllOffersForAClientForAGivenInquiryForAGivenBank(string clientID, int inquiryID, string bankName)
        {
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where (offerSummary.ClientId == clientID
                        && offerSummary.InquiryIdInOurDb == inquiryID
                        && offerSummary.BankName == bankName)
                        select offer;
            return query.ToList();
        }

        public IEnumerable<OfferModel> GetAllOffersForBankEmployee(string bankEmployeeID, string bankName)
        {
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where offerSummary.BankName == bankName
                        select offer;
            return query.ToList();
        }
    }
}
