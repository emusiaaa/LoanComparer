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
            //var result = _context.LoggedInquiries.FirstOrDefault(x => (x.ClientId == userID && x.SubmisionDate.))
            return res;
        }

        public dynamic GetAllForBankEmployee(string bankEmployeeID)
        {

            var query = from loggedInquiry in _context.LoggedInquiries
                        join client in _context.Clients on
                        loggedInquiry.ClientId equals client.Id select new
                        {
                            Id = loggedInquiry.Id,
                            SubmissionDate = loggedInquiry.SubmisionDate,
                            InstallmentsCount = loggedInquiry.InstallmentsCount,
                            LoanValue = loggedInquiry.LoanValue,
                            UserFirstName = client.UserFirstName,
                            UserLastName = client.UserLastName,
                            ClientGovernmentIDNumber = client.ClientGovernmentIDNumber,
                            ClientGovernmentIDType = client.ClientGovernmentIDType,
                            ClientJobType = client.ClientJobType,
                            ClientIncomeLevel = client.ClientIncomeLevel,
                            Email = client.Email
                        };

            return query.AsEnumerable();
        }

    }
}
