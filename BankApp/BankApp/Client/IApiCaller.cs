using System.Net;

namespace BankApp.Client
{
    public interface IApiCaller
    {
        Task<string> PostInquiryAsync(jsonclass.Loan inquiryJson);
        Task<string> GetInquiryAsync(int inquiryId);
        Task<string> GetOfferAsync(int offerId);
        Task<string> GetOfferDetailsAsync(string path);
        Task<bool> SendFileAsync(int offerId, IFormFile formFile);
        void CompleteOfferAsync(int offerId);
    }
}
