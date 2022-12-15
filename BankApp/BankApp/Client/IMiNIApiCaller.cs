﻿namespace BankApp.Client
{
    public interface IMiNIApiCaller
    {
        Task<string> PostInquiryAsync(jsonclass.Loan inquiryJson);
        Task<string> GetInquiryAsync(int inquiryId);
        Task<string> GetOfferAsync(int offerId);
    }
}