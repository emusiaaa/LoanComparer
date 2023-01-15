using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace BankApp.Client
{
    public class MiNIApiCaller : IApiCaller
    {
        private readonly HttpClient _httpClient;
        public MiNIApiCaller(HttpClient client)
        {
            _httpClient = client;
        }
        public async Task<string> PostInquiryAsync(jsonclass.Loan inquiryJson)
        {
            var stringInquiry = JsonConvert.SerializeObject(inquiryJson);
            var httpContent = new StringContent(stringInquiry, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync("/api/v1/Inquire", httpContent);
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsStringAsync();
        }
        public async Task<string> GetInquiryAsync(int inquiryId)
        {
            var result = await _httpClient.GetAsync("/api/v1/Inquire" + $"/{inquiryId}");
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<string> GetOfferAsync(int offerId)
        {
            var result = await _httpClient.GetAsync("/api/v1/Offer" + $"/{offerId}");
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<string> GetOfferDetailsAsync(string path)
        {
            var result = await _httpClient.GetAsync(path);
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<bool> SendFileAsync(int offerId,IFormFile formFile)
        {
            HttpResponseMessage result;
            
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(formFile.OpenReadStream())
                {
                    Headers =
                {
                    ContentLength = formFile.Length,
                    ContentType = new MediaTypeHeaderValue(formFile.ContentType)
                }
                }, "formFile", formFile.FileName);

                result = await _httpClient.PostAsync("/api/v1/Offer" + $"/{offerId}" + "/document/upload", content);
            }

            return result.IsSuccessStatusCode;
        }
        public async void CompleteOfferAsync(int offerId)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/v1/Offer" + $"/{offerId}" + "/complete", JsonConvert.SerializeObject(offerId));
        }

    }
}
