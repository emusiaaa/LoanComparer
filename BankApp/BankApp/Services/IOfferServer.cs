using BankApp.Client;
using BankApp.Models;

namespace BankApp.Services
{
    public interface IOfferServer
    {
        public void SaveOfferForLogged(IApiCaller client, jsonclass.Loan inquiryJson, int inquiryIdInOurDb, string bankName, ClientModel user);
        public void SaveOfferForNotLogged(IApiCaller client, jsonclass.Loan inquiryJson, int inquiryIdInOurDb, string bankName);
    }
}
