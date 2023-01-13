using BankApp.Client;
using BankApp.Models;
using BankApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankApp
{
    static public class Functions
    {
        static async public void SaveOfferForLogged(IMiNIApiCaller client, IOfferRepository _offerRepository, 
            IOffersSummaryRepository _offersSummaryRepository, int inquiryId, int inquiryIdInOurDb, ClientModel user)
        {
            while (true)
            {
                var inquiryContent = await client.GetInquiryAsync(inquiryId);
                var status = JObject.Parse(inquiryContent)["statusDescription"].ToString();
                if (status == "OfferPrepared") break;
                Thread.Sleep(1000);
            }

            var resultContent = await client.GetInquiryAsync(inquiryId);
            var offerId = JObject.Parse(resultContent)["offerId"].ToObject<int>();
            var rOfferContent = await client.GetOfferAsync(offerId);


            var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(rOfferContent);
            //var test = Json(resultContent);
            long offersId = _offerRepository.Add(values);

            string clientID = user.Id;
            string bankName = "projectAPI";

            _offersSummaryRepository.Add(inquiryIdInOurDb,false, bankName, offersId, clientID);
        }
        static async public void SaveOfferForNotLogged(IMiNIApiCaller client, IOfferRepository _offerRepository,
            IOffersSummaryRepository _offersSummaryRepository, int inquiryId, int inquiryIdInOurDb)
        {
            while (true)
            {
                var inquiryContent = await client.GetInquiryAsync(inquiryId);
                var status = JObject.Parse(inquiryContent)["statusDescription"].ToString();
                if (status == "OfferPrepared") break;
                Thread.Sleep(1000);
            }

            var resultContent = await client.GetInquiryAsync(inquiryId);
            var offerId = JObject.Parse(resultContent)["offerId"].ToObject<int>();
            var rOfferContent = await client.GetOfferAsync(offerId);


            var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(rOfferContent);
            //var test = Json(resultContent);
            long offersId = _offerRepository.Add(values);
            string bankName = "projectAPI";

            _offersSummaryRepository.Add(inquiryIdInOurDb, false, bankName, offersId, null);
        }
    }
}
