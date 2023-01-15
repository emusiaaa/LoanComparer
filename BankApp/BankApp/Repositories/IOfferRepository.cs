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
        public OfferModel GetOfferForBankEmployee(int offerIdInOurDb);
        public IEnumerable<OfferModel> GetAllOffersForAGivenInquiry(int inquiryID);
        public IEnumerable<OfferModel> GetAllOffersForAGivenNRInquiry(int inquiryID);

        public string GetOfferBank(int offerId);
        public long Add(Dictionary<string, dynamic> jsonOffer);

        public void Delete(int offerID);
        public void UpdateIsApprovedByEmployee(int offerID, bool decision, string employeeID);
        public OfferModel UpdateIsOfferAccepted(int offerID);
        public int GetOfferIdInBank(long offerId);
    }
}
