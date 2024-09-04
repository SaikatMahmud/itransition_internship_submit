using ArbitraryCollectionMgmt.Web.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ArbitraryCollectionMgmt.Web.Clients
{
    public class SalesforceClient
    {
        private readonly IConfiguration _configuration;

        private const string LoginUrl = "https://itransition27-dev-ed.develop.my.salesforce.com/services/oauth2/token";
        private const string ApiEndpoint = "/services/data/v61.0";
        private string Username { get; set; }
        private string Password { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string AuthToken { get; set; }
        private string InstanceUrl { get; set; }
        private DateTime TokenExpiration { get; set; }
        public SalesforceClient(IConfiguration configuration)
        {
            _configuration = configuration;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        private async Task GetAccessToken()
        {
            if (TokenExpiration > DateTime.Now) return;
            try
            {
                AuthToken = string.Empty;
                string jsonResponseString = string.Empty;
                using (var client = new HttpClient())
                {
                    var request = new FormUrlEncodedContent(
                        new Dictionary<string, string>
                        {
                            {"grant_type", "password"},
                            {"client_id", _configuration["Salesforce:ClientId"]},
                            {"client_secret", _configuration["Salesforce:ClientSecret"]},
                            {"username", _configuration["Salesforce:Username"]},
                            {"password", _configuration["Salesforce:Password"] + _configuration["Salesforce:SecurityToken"]}
                        }
                    );
                    var response = await client.PostAsync(LoginUrl, request);
                    jsonResponseString = await response.Content.ReadAsStringAsync();
                }
                var value = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponseString);
                AuthToken = value["access_token"];
                InstanceUrl = value["instance_url"];
                if (!string.IsNullOrEmpty(AuthToken))
                {
                    TokenExpiration = DateTime.Now.AddMinutes(115); // has 2hr validity
                }
            }
            catch (Exception) { }
        }
        public async Task<string> CreateAccount(SalesforceAccount obj)
        {
            await GetAccessToken();
            string json = JsonSerializer.Serialize(obj);
            using (var client = new HttpClient())
            {
                HttpContent createdContent = new StringContent(json, Encoding.UTF8, "application/json");
                string uri = $"{InstanceUrl}{ApiEndpoint}/sobjects/Account";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                request.Content = createdContent;
                HttpResponseMessage response = client.SendAsync(request).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
        }
    }
}
