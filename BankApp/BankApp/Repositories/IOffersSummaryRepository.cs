using BankApp.Models;

namespace BankApp.Repositories
{
    public interface IOffersSummaryRepository
    {
        public void Add(int inquiryIdInOurDb, bool isNrInquiry, string bankName, long offerIdInOurDb, string? clientId);

        public void Update(int offersSummaryID, OffersSummaryModel offersSummary);

        public void Delete(int offersSummaryID);
    }
}
