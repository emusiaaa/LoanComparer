using BankApp.Models;

namespace BankApp.Repositories
{
    public interface IInquiryRepository
    {
        public InquiryModel GetInquiry(int Id);
        public IEnumerable<InquiryModel> GetAll();
    }
}
