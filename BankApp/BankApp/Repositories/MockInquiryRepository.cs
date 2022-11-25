using BankApp.Models;

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
                new InquiryModel() {LoanValue = 2000, InstallmentsCount= 4, Id =1, SubmisionDate = "01.02.2020"},
                new InquiryModel() {LoanValue = 4000, InstallmentsCount= 3, Id =2, SubmisionDate = "01.02.2020"},
                new InquiryModel() {LoanValue = 9000, InstallmentsCount= 1, Id =3, SubmisionDate = "01.02.2020"},
            };
        }
        public IEnumerable<InquiryModel> GetAll()
        {
            return _inquiry;
        }

        public InquiryModel GetInquiry(int Id)
        {
            return _inquiry.FirstOrDefault(e => e.Id == Id);
        }
    }
}