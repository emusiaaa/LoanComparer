using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace BankApp.Client
{
    public class MiNIHttpClient
    {
        private readonly HttpClient client;
        public MiNIHttpClient(HttpClient client)
        {
            this.client = client;
            //client.DefaultRequestHeaders.Authorization = await GetToken();
        }
        public async Task<AuthenticationHeaderValue> GetToken()
        {
            var clientId = "team4c";
            var clientSecret = "7D84D860-87AC-46AE-B955-68DC7D8C48E3";

            var p = new List<KeyValuePair<string, string>>();
            p.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            p.Add(new KeyValuePair<string, string>("client_id", HttpUtility.UrlEncode(clientId)));
            p.Add(new KeyValuePair<string, string>("client_secret", HttpUtility.UrlEncode(clientSecret)));

            var request = new HttpRequestMessage(HttpMethod.Post, "https://indentitymanager.snet.com.pl/connect/token");
            request.Content = new FormUrlEncodedContent(p);
            request.Headers.Clear();

            HttpResponseMessage response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            // response would be a JSON, just extract token from it
            var accessToken = (string)JToken.Parse(responseBody)["access_token"];
            return new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
