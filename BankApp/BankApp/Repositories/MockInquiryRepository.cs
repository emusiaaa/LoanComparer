using BankApp.Models;

namespace BankApp.Repositories
{
    public class MockInquiryRepository : IInquiryRepository
    {
        private List<InquiryModel> _inquiry;
        public MockInquiryRepository()
        {
            _inquiry = new List<InquiryModel>()
            {
                new InquiryModel() {LoanValue = 2000, InstallmentsCount= 4, FormId =1 },
                new InquiryModel() {LoanValue = 4000, InstallmentsCount= 3,  FormId =2 },
                new InquiryModel() {LoanValue = 9000, InstallmentsCount= 1 , FormId = 3},
            };
        }
        public IEnumerable<InquiryModel> GetAll()
        {
            return _inquiry;
        }

        public InquiryModel GetInquiry(int Id)
        {
            return _inquiry.FirstOrDefault(e=> e.FormId == Id);
        }
    }
}
