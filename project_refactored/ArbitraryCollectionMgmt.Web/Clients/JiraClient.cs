using ArbitraryCollectionMgmt.BLL.DTOs;
using Azure;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ArbitraryCollectionMgmt.Web.Clients
{
    public class JiraClient
    {
        private readonly IConfiguration _configuration;
        private const string JiraUri = "https://saikatdev67-itransition.atlassian.net";
        private const string JiraIssueUri = "https://saikatdev67-itransition.atlassian.net/rest/api/3/issue";
        private const string JiraUserUri = "https://saikatdev67-itransition.atlassian.net/rest/api/3/user";
        private const string JiraSearchIssueUri = "https://saikatdev67-itransition.atlassian.net/rest/api/3/search";
        private const string ProjectKey = "SUP";
        public JiraClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            var authData = Encoding.UTF8.GetBytes(_configuration["Jira:Username"] + ":" + _configuration["Jira:ApiKey"]);
            var basicAuthentication = Convert.ToBase64String(authData);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthentication);
            return client;
        }


        /* private async Task<string> CheckUserExists(string email)
         {
             var client = GetHttpClient();
             var result = await client.GetAsync($"{JiraUserUri}/search?query={email}");
             if (result.StatusCode == System.Net.HttpStatusCode.OK)
             {
                 var response = await result.Content.ReadAsStringAsync();
                 if (!string.IsNullOrEmpty(response) && response.Contains("accountId"))
                 {
                     dynamic json = JArray.Parse(response)[0];
                     return json.accountId.ToString();
                 }
                 return string.Empty;
             }
             else
                 throw new Exception(await result.Content.ReadAsStringAsync());
         }*/

        private async Task<string> GetAccountId(string email)
        {
            //var accountId = await CheckUserExists(email);
            //if (!string.IsNullOrEmpty(accountId)) return accountId;

            /*********  No need to check existing users. Jira will return existing Id if a user exists  ********/

            var userData = new
            {
                emailAddress = email,
                products = new object[] { },
            };
            var client = GetHttpClient();
            var content = new StringContent(JsonSerializer.Serialize(userData), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(JiraUserUri, content);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var response = await result.Content.ReadAsStringAsync();
                dynamic json = JObject.Parse(response);
                return json.accountId.ToString();
            }
            return string.Empty;
        }

        public async Task<bool> CreateTicket(string email, string summary, string priority, string fromUrl)
        {
            var accountId = await GetAccountId(email);
            if (string.IsNullOrEmpty(accountId)) return false;
            var data = new
            {
                fields = new
                {
                    project = new { key = ProjectKey },
                    summary = summary,
                    issuetype = new { name = "Task" },
                    /* description = new
                     {
                         version = 1,
                         type = "doc",
                         content = new[] {
                             new {
                                 type = "paragraph",
                                 content = new []{
                                     new {
                                         type = "text",
                                         text =  description
                                     }
                                 }
                             }
                         }
                     },*/
                    customfield_10064 = fromUrl, //invoked from which page inside the app
                    customfield_10010 = "sup/290c0e36-e84d-4225-8f4c-d8c36200847a", //request type "Customer"
                    priority = new { name = priority },
                    reporter = new { accountId = accountId }
                }
            };
            var client = GetHttpClient();
            var result = await client.PostAsJsonAsync(JiraIssueUri, data);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
                return true;
            //return await result.Content.ReadFromJsonAsync<JiraTicketResponse>();
            else
                return false;
            //throw new Exception(await result.Content.ReadAsStringAsync());

        }
        /*public class JiraTicketResponse
        {
            public int Id { get; set; }
            public string Key { get; set; }
            public Uri Self { get; set; }
        }*/
        public async Task<string> GetUserTicketList(string email, int startAt, int maxResults)
        {
            var data = new
            {
                jql = $"reporter = '{email}' order by created DESC",
                startAt = startAt,
                maxResults = maxResults,
                fields = new[] { "key", "summary", "status", "customfield_10064", "created" }
            };
            var client = GetHttpClient();
            var result = await client.PostAsJsonAsync(JiraSearchIssueUri, data);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return string.Empty;
            }
            var response = await result.Content.ReadAsStringAsync();
            return response;
        }

    }
}
