//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json.Linq;

//namespace BankApp
//{
//    static public class Functions
//    {
//        static public async void WaitingForOffer(HttpClient client, int id)
//        {
//            while (true)
//            {
//                var result = await client.GetAsync("/api/v1/Inquire/" + $"/{id}");
//                var resultContent = await result.Content.ReadAsStringAsync();
//                var status = JObject.Parse(resultContent)["statusDescription"];
//                //if(status == "OfferPrepared")
//                Thread.Sleep(1000);
//            }
//        }
//    }
//}
