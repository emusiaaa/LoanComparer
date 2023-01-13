using BankApp.Client;
using BankApp.Models;
using BankApp.Repositories;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using BankApp.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.Services
{
    public class OfferServer: IOfferServer
    {
        //private readonly IOffersSummaryRepository _offersSummaryRepository;
        //private readonly IOfferRepository _offerRepository;
        private readonly IServiceProvider _serviceProvider;
        public OfferServer(IServiceProvider serviceProvider/*, IOffersSummaryRepository offersSummaryRepository, IOfferRepository offerRepository*/)
        {
            //_offersSummaryRepository = offersSummaryRepository;
            //_offerRepository = offerRepository;
            _serviceProvider = serviceProvider;
        }
    
        async public void SaveOfferForLogged(IMiNIApiCaller client, int inquiryId, int inquiryIdInOurDb, ClientModel user)
        {
            long offersId;
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
            using (var scope = _serviceProvider.CreateScope())
            {
                var _offerRepository = scope.ServiceProvider.GetRequiredService<IOfferRepository>();
                // do something with context
                offersId = _offerRepository.Add(values);
            }
            

            string clientID = user.Id;
            string bankName = "projectAPI";
            using (var scope = _serviceProvider.CreateScope())
            {
                var _offersSummaryRepository = scope.ServiceProvider.GetRequiredService<IOffersSummaryRepository>();
                // do something with context
                _offersSummaryRepository.Add(inquiryIdInOurDb, false, bankName, offersId, clientID);
            }

            //_offersSummaryRepository.Add(inquiryIdInOurDb, false, bankName, offersId, clientID);
        }
        async public void SaveOfferForNotLogged(IMiNIApiCaller client, int inquiryId, int inquiryIdInOurDb)
        {
            long offersId;
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
            using (var scope = _serviceProvider.CreateScope())
            {
                var _offerRepository = scope.ServiceProvider.GetRequiredService<IOfferRepository>();
                // do something with context
                offersId = _offerRepository.Add(values);
            }
            //long offersId = _offerRepository.Add(values);
            string bankName = "projectAPI";
            using (var scope = _serviceProvider.CreateScope())
            {
                var _offersSummaryRepository = scope.ServiceProvider.GetRequiredService<IOffersSummaryRepository>();
                // do something with context
                _offersSummaryRepository.Add(inquiryIdInOurDb, true, bankName, offersId, null);
            }
            // _offersSummaryRepository.Add(inquiryIdInOurDb, false, bankName, offersId, null);
        }
    }
}
