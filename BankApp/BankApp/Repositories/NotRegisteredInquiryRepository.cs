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
        public IEnumerable<AllInquiryViewModel> ToAllInquiryModel()
        {
            var res = _context.NotRegisteredInquiries
                //.Where(s => s.ClientId == bankEmployeeID)
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
                    Email = i.Email
                })
                .ToList();
            return res;
        }
        public IEnumerable<AllInquiryViewModel> ToAllInquiryModelFilteredByName(string filter, int dateRange)
        {
            if (filter == null) filter = "";
            var res = _context.NotRegisteredInquiries
                .Where(
                dateRange == 0 ? 
                (i => i.UserFirstName.Contains(filter) || i.UserLastName.Contains(filter)) 
                : (i => (i.UserFirstName.Contains(filter) || i.UserLastName.Contains(filter)) && (DateTime)(object)i.SubmissionDate >= DateTime.Now.AddDays(-dateRange)))
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
                    Email = i.Email
                })
                .ToList();
            return res;
        }
    }
}
