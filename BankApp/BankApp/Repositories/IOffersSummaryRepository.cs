using BankApp.Models;

namespace BankApp.Repositories
{
    public interface IOffersSummaryRepository
    {
        public void Add(OffersSummaryModel offersSummary);

        public void Update(int offersSummaryID, OffersSummaryModel offersSummary);

        public void Delete(int offersSummaryID);
    }
}
