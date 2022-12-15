using BankApp.Models;

namespace BankApp.Repositories
{
    public interface INotRegisteredInquiryRepository
    {
        public NotRegisteredInquiryModel Get(int inquiryID);

        public int Add(NotRegisteredInquiryModel notRegisteredInquiry);

        public void Update(int inquiryID, NotRegisteredInquiryModel notRegisteredInquiry);
        public IEnumerable<NotRegisteredInquiryModel> GetAllForBankEmployee(string bankEmployeeID);
        public IEnumerable<AllInquiryViewModel> ToAllInquiryModel();
        public IEnumerable<AllInquiryViewModel> ToAllInquiryModelFilteredByName(string filter, int dateRange);
        public void Delete(int inquiryID);
    }
}