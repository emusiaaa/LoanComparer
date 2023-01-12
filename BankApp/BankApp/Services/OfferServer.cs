using BankApp.Client;
using BankApp.Models;
using BankApp.Repositories;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BankApp.Services
{
    public class OfferServer: IOfferServer
    {
        private readonly IOffersSummaryRepository _offersSummaryRepository;
        private readonly IOfferRepository _offerRepository;
        public OfferServer(IOffersSummaryRepository offersSummaryRepository, IOfferRepository offerRepository)
        {
            _offersSummaryRepository = offersSummaryRepository;
            _offerRepository = offerRepository;
        }
    
        async public void SaveOfferForLogged(IMiNIApiCaller client, int inquiryId, int inquiryIdInOurDb, ClientModel user)
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

            _offersSummaryRepository.Add(inquiryIdInOurDb, false, bankName, offersId, clientID);
        }
        async public void SaveOfferForNotLogged(IMiNIApiCaller client, int inquiryId, int inquiryIdInOurDb)
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
