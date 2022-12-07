using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace BankApp
{
    static public class Functions
    {
        static public void WaitingForOffer(Object obj)
        {
            HttpClient client = (HttpClient)obj;
            while (true)
            {
                //var result = await client.GetAsync("/api/v1/Inquire/38");
                //var resultContent = await result.Content.ReadAsStringAsync();
                //var status = JObject.Parse(resultContent)["statusDescription"].ToString();
                //if (status == "OfferPrepared") break;
                Thread.Sleep(1000);
                break;
            }
        }
    }
}
