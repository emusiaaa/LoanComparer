using BankApp.Models;

namespace BankApp.Repositories
{
    public interface INotRegisteredInquiryRepository
    {
        public NotRegisteredInquiryModel Get(int inquiryID);

        public void Add(NotRegisteredInquiryModel notRegisteredInquiry);

        public void Update(int inquiryID, NotRegisteredInquiryModel notRegisteredInquiry);


        public void Delete(int inquiryID);
    }
}