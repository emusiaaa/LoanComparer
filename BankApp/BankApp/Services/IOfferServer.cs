using BankApp.Client;
using BankApp.Models;

namespace BankApp.Services
{
    public interface IOfferServer
    {
        public void SaveOfferForLogged(IMiNIApiCaller client, int inquiryId, int inquiryIdInOurDb, ClientModel user);
        public void SaveOfferForNotLogged(IMiNIApiCaller client, int inquiryId, int inquiryIdInOurDb);
    }
}
