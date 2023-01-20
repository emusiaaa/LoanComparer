﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.IO;

namespace BankApp.Client
{
    public class BestAPIApiCaller : IApiCaller
    {
        private readonly HttpClient _httpClient;
        public BestAPIApiCaller(HttpClient client)
        {
            _httpClient = client;
        }
        public async Task<string> PostInquiryAsync(jsonclass.Loan inquiryJson)
        {
            var stringInquiry = JsonConvert.SerializeObject(inquiryJson);
            var httpContent = new StringContent(stringInquiry, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync("/api/v1/Inquire", httpContent);
            if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                GetToken();
                httpResponse = await _httpClient.PostAsync("/api/v1/Inquire", httpContent);
            }
            return await httpResponse.Content.ReadAsStringAsync();
        }
        public async Task<string> GetInquiryAsync(int inquiryId)
        {
            var result = await _httpClient.GetAsync("/api/v1/Inquire" + $"/{inquiryId}");
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                GetToken();
                result = await _httpClient.GetAsync("/api/v1/Inquire" + $"/{inquiryId}");
            }
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> GetOfferAsync(int offerId)
        {
            var result = await _httpClient.GetAsync("/api/v1/Offer" + $"/{offerId}");
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                GetToken();
                result = await _httpClient.GetAsync("/api/v1/Offer" + $"/{offerId}");
            }
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> GetOfferDetailsAsync(string path)
        {
            var result = await _httpClient.GetAsync(path);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                GetToken();
                result = await _httpClient.GetAsync(path);
            }
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<bool> SendFileAsync(int offerId, IFormFile formFile)
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
                }, "usersFormFile", formFile.FileName);

                result = await _httpClient.PostAsync("/api/v1/Offer" + $"/{offerId}" + "/document/upload", content);
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    GetToken();
                    result = await _httpClient.PostAsync("/api/v1/Offer" + $"/{offerId}" + "/document/upload", content);
                }
            }

            return result.IsSuccessStatusCode;
        }
        public async void CompleteOfferAsync(int offerId)
        {
            GetToken();
            var result = await _httpClient.PostAsync("/api/v1/Offer" + $"/{offerId}" + "/complete", null);
        }
        private void GetToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/Authenticate");
            var myData = new
            {
                username = "teamBestBankApi",
                password = "zaq1@WSX"
            };
            string jsonData = JsonConvert.SerializeObject(myData);
            request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            var r = response.Content.ReadAsStringAsync();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", JObject.Parse(r.Result)["token"].ToString());
        }

    }
}
