using BankApp.Models;

namespace BankApp.Repositories
{
    public interface IOfferRepository
    {
        public IEnumerable<OfferModel> GetAllOffersForAClientAllInquiries(string clientID);

        public IEnumerable<OfferModel> GetAllOffersForAClientForAGivenInquiry(string clientID, int inquiryID);

        public IEnumerable<OfferModel> GetAllOffersForBankEmployee(string bankEmployeeID, string bankName);

        public OfferModel GetAllOffersForAClientForAGivenInquiryForAGivenBank(string clientID, int inquiryID, string bankName);

        public OfferModel GetAllOffersForAClientForAGivenInquiryForAGivenBank(int offerIdInBank, string bankName);

        public long Add(Dictionary<string, dynamic> jsonOffer);

        public void Delete(int offerID);

        public IEnumerable<OfferModel> GetAllOffersForClientInquiry(int inquiryId);
    }
}
