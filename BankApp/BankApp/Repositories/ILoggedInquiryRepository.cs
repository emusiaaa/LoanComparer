using BankApp.Models;

namespace BankApp.Repositories
{
    public interface ILoggedInquiryRepository
    {
        public InquiryModel Get(int inquiryID);

        public void Add(InquiryModel client);

        public void Update(int clientID, InquiryModel client);

        public void Delete(int clientID);
        public IEnumerable<InquiryModel> GetAll(string userID);

        public IEnumerable<InquiryModel> GetAllForBankEmployee(string bankEmployeeID);
        public IEnumerable<InquiryModel> GetAllByDateDesc(string bankEmployeeID);
        public IEnumerable<InquiryModel> GetAllByLoanDesc(string bankEmployeeID);
        public IEnumerable<InquiryModel> GetAllByLoanAsc(string bankEmployeeID);
        public IEnumerable<InquiryModel> FilterName(string bankEmployeeID, string searchString);
    }
}
