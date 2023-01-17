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

        public void Add(int inquiryIdInOurDb, bool isNrInquiry, string bankName, long offerIdInOurDb, string? clientId)
        {
            OffersSummaryModel offersSummary = new OffersSummaryModel();
            offersSummary.OfferIdInOurDb = offerIdInOurDb;
            offersSummary.ClientId = clientId;
            offersSummary.BankName = bankName; 
            offersSummary.IsNRInquiry= isNrInquiry;
            offersSummary.InquiryIdInOurDb = inquiryIdInOurDb;
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
        public NRmodel GetInquiryIdInOurDb(int offerId)
        {
            var res = _context.OffersSummary.Where(o => o.OfferIdInOurDb == offerId).FirstOrDefault();
            if (res != null) {
                var m = new NRmodel();
                m.isNR = res.IsNRInquiry;
                m.id = res.InquiryIdInOurDb;
                return m;
            }
            else return new NRmodel();
        }
    }
}
