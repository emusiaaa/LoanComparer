namespace BankApp.Models
{
    public class InquiryString
    {
        public int? inquiryId { get; set; }
        public int? inquiryIdInOurDb { get; set; }
        public string? urlAddress { get; set; }
        public bool? isNR { get; set; }
    }
    public class OfferString
    {
        public string? offerString { get; set; }
    }
}
