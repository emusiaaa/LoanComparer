namespace BankApp.Models
{
    public class AllInquiryViewModel
    {
        public IEnumerable<NotRegisteredInquiryModel> NotRegisteredInquiries { get; set; }  
        public dynamic LoggedInquiriesFullData { get; set; }
    }
}
