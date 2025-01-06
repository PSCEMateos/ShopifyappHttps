using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace ShopifyAppSherable.Services
{
    public class CreateGraphQLConnection 
    {
        private readonly string _StoreName;
        private readonly string _access_token;
        private readonly string _apiVersion;
        private readonly string _BuildApiUrl = "";
        public CreateGraphQLConnection(string store, string token, string apiVersion) 
        {
            _access_token = token;
            _apiVersion = apiVersion;
            _StoreName = store;
            _BuildApiUrl = $"https://{_StoreName}.myshopify.com/admin/api/{_apiVersion}/graphql.json";
        }
         
        public async Task<string> webhookSubscriptionRequest()
        {
            using HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, _BuildApiUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("X-Shopify-Access-Token", _access_token);
            request.Headers.Add("Content-Type", "application/json");
            var message = new
            {
                query = "query { webhookSubscription(id: \"gid://shopify/WebhookSubscription/892403750\") { id topic endpoint { __typename ... on WebhookHttpEndpoint { callbackUrl } ... on WebhookEventBridgeEndpoint { arn } } } }",
            };
            request.Content = new StringContent(JsonSerializer.Serialize(message));

            HttpResponseMessage response = await client.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            return "Response" + responseBody;
        }
    }
}