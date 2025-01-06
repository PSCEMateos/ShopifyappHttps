using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using ShopifyAppSherable.Services;

namespace ShopifyAppSherable.Services
{
    public class ShpifyWebhookService
    {
    }
    public class WebhookVerification
    {
        private readonly string _shopifySecret;
        private readonly string _accessToken;
        private readonly string _graphqlEndpoint;
        public WebhookVerification(string shopifySecret, string storeName, string accessToken, string apiVersion)
        {
            _shopifySecret = shopifySecret;
            _accessToken = accessToken;
            _graphqlEndpoint = $"https://{storeName}.myshopify.com/admin/api/{apiVersion}/graphql.json";
        }

        public bool VerifySignature(string body, string realHmac)
        {
            var secret = Encoding.UTF8.GetBytes(_shopifySecret);
            using var hmac = new HMACSHA256(secret);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
            var calculatedHmac = Convert.ToBase64String(hash);
            return calculatedHmac == realHmac;
        }
        public async Task<string> SubscribeToWebhookAsync(string topico, string urlRecibir)
        {
            string formatJson = "JSON";
            string filterUse = null;

            var mutation = @"
        mutation webhookSubscriptionCreate($topic: WebhookSubscriptionTopic!, $webhookSubscription: WebhookSubscriptionInput!) {
            webhookSubscriptionCreate(topic: $topic, webhookSubscription: $webhookSubscription) {
                webhookSubscription {
                    id
                    topic
                    filter
                    format
                    endpoint {
                        __typename
                        ... on WebhookHttpEndpoint {
                            callbackUrl
                        }
                    }
                }
                userErrors {
                    field
                    message
                }
            }
        }";

            var variableJson = new
            {
                topic = topico,
                webhookSubscription = new
                {
                    callbackUrl = urlRecibir,
                    format = formatJson,
                    filter = filterUse
                }
 
            };

            var payload = new
            {
                query = mutation,
                variables = variableJson
            };

            try
            {
                var jsonRequest = JsonSerializer.Serialize(payload);

                using HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, _graphqlEndpoint);

                request.Headers.Add("X-Shopify-Access-Token", _accessToken);
                request.Content = new StringContent(jsonRequest);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                return "Response: " + responseBody;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

    }
}
