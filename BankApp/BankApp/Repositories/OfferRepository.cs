﻿using BankApp.Data;
using BankApp.Models;

namespace BankApp.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly LoansComparerDBContext _context;

        public OfferRepository(LoansComparerDBContext context)
        {
            _context = context;
        }

        public void Add(OfferModel offer)
        {
            _context.Offers.Add(offer);
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
