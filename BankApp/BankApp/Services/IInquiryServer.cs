using BankApp.Models;

namespace BankApp.Services
{
    public interface IInquiryServer
    {
        public jsonclass.Loan CreateNRInquiry(NotRegisteredInquiryModel inquiry);
        public jsonclass.Loan CreateInquiry(InquiryModel inquiry, ClientModel user);
    }
}
