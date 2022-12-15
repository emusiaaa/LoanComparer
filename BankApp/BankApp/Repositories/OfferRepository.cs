﻿using BankApp.Data;
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

        public long Add(Dictionary<string, dynamic> jsonOffer)
        {
            OfferModel offer = new OfferModel();

            var x = jsonOffer["createDate"];
            var y = jsonOffer["updateDate"];
            var z = jsonOffer["documentLinkValidDate"];

            offer.OfferIdInBank = (int)jsonOffer["id"];
            offer.Percentage = (double)jsonOffer["percentage"];
            offer.MonthlyInstallment = (double)jsonOffer["monthlyInstallment"];
            offer.RequestedValue = (double)jsonOffer["requestedValue"];
            offer.RequestedPeriodInMonth = (int)jsonOffer["requestedPeriodInMonth"];
            offer.StatusId = (int)jsonOffer["statusId"];
            offer.StatusDescription = (string)jsonOffer["statusDescription"];
            offer.InquireId = (int)jsonOffer["inquireId"];
            offer.CreateDate = new DateTime((int)x.Year, (int)x.Month, (int)x.Day, (int)x.Hour, (int)x.Minute, (int)x.Second, DateTimeKind.Utc).ToString("o"); 
            offer.UpdateDate = new DateTime((int)y.Year, (int)y.Month, (int)y.Day, (int)y.Hour, (int)y.Minute, (int)y.Second, DateTimeKind.Utc).ToString("o");
            offer.ApprovedBy = (int?)jsonOffer["approvedBy"];
            offer.DocumentLink = (string)jsonOffer["documentLink"];
            offer.DocumentLinkValidDate = new DateTime((int)z.Year, (int)z.Month, (int)z.Day, (int)z.Hour, (int)z.Minute, (int)z.Second, DateTimeKind.Utc).ToString("o"); ;

            var res = _context.Offers.Add(offer); // może da się stąd wyciągnąć id stworzonej oferty i zwrócić z Add żeby OffersSummary miało??
            _context.SaveChanges();

            return offer.Id;
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

        public OfferModel GetAllOffersForAClientForAGivenInquiryForAGivenBank(string clientID, int inquiryID, string bankName)
        {
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where (offerSummary.ClientId == clientID
                        && offerSummary.InquiryIdInOurDb == inquiryID
                        && offerSummary.BankName == bankName)
                        select offer;
            return query.FirstOrDefault();
        }

        public OfferModel GetAllOffersForAClientForAGivenInquiryForAGivenBank(int offerIdInBank , string bankName)
        {
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where (
                        offer.OfferIdInBank == offerIdInBank
                        && offerSummary.BankName == bankName)
                        select offer;
            return query.FirstOrDefault();
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