using BankApp.Data;
using BankApp.Models;

namespace BankApp.Repositories
{
    public class NotRegisteredInquiryRepository: INotRegisteredInquiryRepository
    {
        private readonly LoansComparerDBContext _context;

        public NotRegisteredInquiryRepository(LoansComparerDBContext context)
        {
            _context = context;
        }

        public NotRegisteredInquiryModel Get(int inquiryID) => _context.NotRegisteredInquiries.SingleOrDefault(x => x.Id ==inquiryID);

        public void Add(NotRegisteredInquiryModel inquiry)
        {
            _context.NotRegisteredInquiries.Add(inquiry);
            _context.SaveChanges();
        }

        public void Update(int inquiryID, NotRegisteredInquiryModel inquiry)
        {
            var result = _context.NotRegisteredInquiries.SingleOrDefault(x => x.Id == inquiryID);
            if (result != null)
            {
                result.Email = inquiry.Email;
                result.LoanValue = inquiry.LoanValue;
                result.InstallmentsCount = inquiry.InstallmentsCount;

                _context.SaveChanges();
            }
        }

        public void Delete(int inquiryID)
        {
            var result = _context.NotRegisteredInquiries.SingleOrDefault(x => x.Id == inquiryID);
            if (result != null)
            {
                _context.NotRegisteredInquiries.Remove(result);
                _context.SaveChanges();
            }
        }

        public IEnumerable<NotRegisteredInquiryModel> GetAllForBankEmployee(string bankEmployeeID)
        {
            var res = _context.NotRegisteredInquiries.ToList();
            return res;
        }
    }
}
