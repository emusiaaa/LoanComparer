using BankApp.Data;
using BankApp.Models;
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

        public IEnumerable<InquiryModel> GetAllForBankEmployee(string bankEmployeeID)
        {
            var res = _context.LoggedInquiries.ToList();
            return res;
        }

    }
}
