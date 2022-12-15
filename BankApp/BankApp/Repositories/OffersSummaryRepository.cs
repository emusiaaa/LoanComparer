using BankApp.Data;
using BankApp.Models;

namespace BankApp.Repositories
{
    public class OffersSummaryRepository : IOffersSummaryRepository
    {
        private readonly LoansComparerDBContext _context;

        public OffersSummaryRepository(LoansComparerDBContext context)
        {
            _context = context;
        }

        public void Add(OffersSummaryModel offersSummary)
        {
            _context.OffersSummary.Add(offersSummary);
            _context.SaveChanges();
        }

        public void Delete(int offersSummaryID)
        {
            var result = _context.OffersSummary.SingleOrDefault(x => x.Id == offersSummaryID);
            if (result != null)
            {
                _context.OffersSummary.Remove(result);
                _context.SaveChanges();
            }
        }
        public void Update(int offersSummaryID, OffersSummaryModel offersSummary)
        {
            var result = _context.OffersSummary.SingleOrDefault(x => x.Id == offersSummaryID);
            if (result != null)
            {
                result.OfferIdInOurDb = offersSummary.OfferIdInOurDb;
                result.InquiryIdInOurDb = offersSummary.InquiryIdInOurDb;
                result.ClientId = offersSummary.ClientId;
                result.BankName = offersSummary.BankName;
                result.IsNRInquiry = offersSummary.IsNRInquiry;

                _context.SaveChanges();
            }
        }
    }
}
