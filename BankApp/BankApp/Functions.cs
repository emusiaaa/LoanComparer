using BankApp.Client;
using BankApp.Models;
using BankApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace BankApp
{
    static public class Functions
    {
        //static public string GetToken(HttpRequestHeader token)
        //{
        //    if(token != null)
        //    {
        //        return token;
        //    }
        //    else
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri("https://best-bank-webapi.azurewebsites.net");
        //            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/Authenticate");
        //            var myData = new
        //            {
        //                username = "teamBestBankApi",
        //                password = "zaq1@WSX"
        //            };
        //            string jsonData = JsonConvert.SerializeObject(myData);
        //            request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        //            //request.Content.Headers.ContentType =
        //            //request.Headers.

        //            HttpResponseMessage response = client.Send(request);
        //            response.EnsureSuccessStatusCode();
        //            var r = response.Content.ReadAsStringAsync();
        //            return JObject.Parse(r.Result)["token"].ToString();
        //        }
        //    }
        //}
    }
}
