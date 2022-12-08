using BankApp.Data;
using BankApp.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.Design;

namespace BankApp.Repositories
{
    public class LoggedInquiryRepository : ILoggedInquiryRepository
    {
        private readonly LoansComparerDBContext _context;
        public LoggedInquiryRepository(LoansComparerDBContext context)
        {
            _context = context;
        }
        public InquiryModel Get(int inquiryID)
               => _context.LoggedInquiries.SingleOrDefault(x => x.Id == inquiryID);

        public void Add(InquiryModel inquiry)
        {
            _context.LoggedInquiries.Add(inquiry);
            _context.SaveChanges();
        }
        public void Update(int inquiryID, InquiryModel inquiry)
        {
            var result = _context.LoggedInquiries.SingleOrDefault(x => x.Id == inquiryID);
            if (result != null)
            {
                result.LoanValue = inquiry.LoanValue;
                result.InstallmentsCount = inquiry.InstallmentsCount;
                result.ClientId = inquiry.ClientId;

                _context.SaveChanges();
            }
        }
        public void Delete(int inquiryID)
        {
            var result = _context.LoggedInquiries.SingleOrDefault(x => x.Id == inquiryID);
            if (result != null)
            {
                _context.LoggedInquiries.Remove(result);
                _context.SaveChanges();
            }
        }
        public IEnumerable<InquiryModel> GetAll(string userID)
        {
            var res = _context.LoggedInquiries
                 .Where(s => s.ClientId == userID &&
                             (DateTime)(object)s.SubmisionDate >= DateTime.Now.AddDays(-30))
                 .ToList();
            return res;
        }
        public IEnumerable<AllInquiryViewModel> ToAllInquiryModel()
        {
            var res = _context.LoggedInquiries
                 .Join(_context.Clients, i => i.ClientId, c => c.Id, 
                 (i,c) => new AllInquiryViewModel
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
                     Email = c.Email
                 })
                 .ToList();
            return res;
        }
        public IEnumerable<AllInquiryViewModel> ToAllInquiryModelFilteredByName(string filter, int dateRange)
        {
            if (filter == null) filter = "";
            var res = _context.LoggedInquiries
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
                     Email = c.Email
                 })
                 .Where(
                     dateRange == 0 ?
                    (i => i.UserFirstName.Contains(filter) || i.UserLastName.Contains(filter))
                     : (i => (i.UserFirstName.Contains(filter) || i.UserLastName.Contains(filter)) && (DateTime)(object)i.SubmissionDate >= DateTime.Now.AddDays(-dateRange)))
                 .ToList();
            return res;
        }
    }
}
