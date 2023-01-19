using BankApp.Data;
using BankApp.Models;
using System;
using System.Drawing;
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
            offer.CreateDate = typeof(string)==x.GetType()?x:new DateTime((int)x.Year, (int)x.Month, (int)x.Day, (int)x.Hour, (int)x.Minute, (int)x.Second, DateTimeKind.Utc).ToString("o"); 
            offer.UpdateDate = typeof(string) == y.GetType()?y:new DateTime((int)y.Year, (int)y.Month, (int)y.Day, (int)y.Hour, (int)y.Minute, (int)y.Second, DateTimeKind.Utc).ToString("o");
            offer.ApprovedBy = (string?)jsonOffer["approvedBy"];
            offer.DocumentLink = (string)jsonOffer["documentLink"];
            offer.DocumentLinkValidDate = typeof(string) == z.GetType()?z:new DateTime((int)z.Year, (int)z.Month, (int)z.Day, (int)z.Hour, (int)z.Minute, (int)z.Second, DateTimeKind.Utc).ToString("o"); ;
            offer.IsOfferAccepted = false;

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

        public IEnumerable<OfferModel> GetAllOffersForClientInquiry(int inquiryId)
        {
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where offerSummary.InquiryIdInOurDb == inquiryId
                        select offer;
            return query.ToList();
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
        public IEnumerable<OfferModel> GetAllOffersForAGivenInquiry(int inquiryID)
        {
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where (offerSummary.InquiryIdInOurDb == inquiryID && offerSummary.IsNRInquiry ==false)
                        select offer;
            return query.ToList();
        }
        public IEnumerable<OfferModel> GetAllOffersForAGivenNRInquiry(int inquiryID)
        {
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where (offerSummary.InquiryIdInOurDb == inquiryID && offerSummary.IsNRInquiry==true)
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
                        where (offerSummary.BankName == bankName
                        && offer.IsOfferAccepted == true)
                        select offer;
            return query.ToList();
        }
        public OfferModel GetOfferForBankEmployee(int offerIdInOurDb)
        {
            return _context.Offers.Where(o => o.Id == offerIdInOurDb).FirstOrDefault();
        }
        public void UpdateIsApprovedByEmployee(int offerID, bool decision, string employeeID)
        {
            var result = _context.Offers.SingleOrDefault(x => x.Id == offerID);
            if (result != null)
            {
                result.IsApprovedByEmployee = decision;
                result.ApprovedBy = employeeID;
                result.StatusDescription = decision ? "Accepted" : "Declined";
            }
            _context.SaveChanges();
        }
        public OfferModel UpdateIsOfferAccepted(int offerID)
        {
            var result = _context.Offers.SingleOrDefault(x => x.Id == offerID);
            if (result != null)
            {
                result.IsOfferAccepted = true;
            }
            _context.SaveChanges();
            return result;
        }
        public string GetOfferBank(int offerId)
        {
            var c = _context.OffersSummary.Where(o => o.OfferIdInOurDb == offerId).FirstOrDefault();
            return c.BankName;
        }
        public int GetOfferIdInBank(long offerId)
        {
            var c = _context.Offers.SingleOrDefault(o => o.Id == offerId);
            return c.OfferIdInBank;
        }
        public OfferDetailsForBankEmployee OfferDetailsForBankEmployeeType(int offerId)
        {
            var model = new OfferDetailsForBankEmployee();
            var query = from offerSummary in _context.OffersSummary
                        join offer in _context.Offers
                        on offerSummary.OfferIdInOurDb equals offer.Id
                        where (offer.Id == offerId)
                        select new {offer, offerSummary};
            var first = query.FirstOrDefault();
            if (first != null && first.offerSummary.IsNRInquiry)
            {
                model.offer = first.offer;
                var i = _context.NotRegisteredInquiries
                .Where(s => s.Id == first.offerSummary.InquiryIdInOurDb)
                .Select(i => new AllInquiryViewModel
                {
                    SubmissionDate = i.SubmissionDate,
                    InstallmentsCount = i.InstallmentsCount,
                    LoanValue = i.LoanValue,
                    UserFirstName = i.UserFirstName,
                    UserLastName = i.UserLastName,
                    ClientGovernmentIDNumber = i.ClientGovernmentIDNumber,
                    ClientGovernmentIDType = i.ClientGovernmentIDType,
                    ClientJobType = i.ClientJobType,
                    ClientIncomeLevel = i.ClientIncomeLevel,
                    Email = i.Email,
                    UserBirthDay = i.UserBirthDay,
                    ClientJobEndDay = i.ClientJobEndDay,
                    ClientJobStartDay = i.ClientJobStartDay
                })
                .FirstOrDefault();
                if (i != null) model.inquiry = i;
            }
            else
            {
               model.offer = first.offer;
               var i = _context.LoggedInquiries
               .Where(i => i.Id == first.offerSummary.InquiryIdInOurDb)
               .Join(_context.Clients, i => i.ClientId, c => c.Id,
               (i, c) => new AllInquiryViewModel
               {
                   SubmissionDate = i.SubmisionDate,
                   InstallmentsCount = i.InstallmentsCount,
                   LoanValue = i.LoanValue,
                   UserFirstName = c.UserFirstName,
                   UserLastName = c.UserLastName,
                   ClientGovernmentIDNumber = c.ClientGovernmentIDNumber,
                   ClientGovernmentIDType = c.ClientGovernmentIDType,
                   ClientJobType = c.ClientJobType,
                   ClientIncomeLevel = c.ClientIncomeLevel,
                   Email = c.Email,
                   UserBirthDay = c.UserBirthDay,
                   ClientJobEndDay = c.ClientJobEndDay,
                   ClientJobStartDay = c.ClientJobStartDay
               }).FirstOrDefault();
                if (i != null) model.inquiry = i;
            }          
            return model;
        }
    }
}
