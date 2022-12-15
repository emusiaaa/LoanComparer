using BankApp.Models;

namespace BankApp.Repositories
{
    public interface IOfferRepository
    {
        public IEnumerable<OfferModel> GetAllOffersForAClientAllInquiries(string clientID);

        public IEnumerable<OfferModel> GetAllOffersForAClientForAGivenInquiry(string clientID, int inquiryID);

        public IEnumerable<OfferModel> GetAllOffersForBankEmployee(string bankEmployeeID, string bankName);

        public void Add(OfferModel offer);

        public void Delete(int offerID);
    }
}
