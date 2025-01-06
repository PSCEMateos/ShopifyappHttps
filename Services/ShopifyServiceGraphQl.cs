using System.Text.Json;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System.Text;

namespace ShopifyAppSherable.Services
{
    public class ShopifyServiceGraphQl
    {
        private readonly string _storeName;
        private readonly string _accessToken;
        private readonly string _apiVersion;
        private readonly string _graphqlEndpoint;
        private readonly HttpClient _httpClient;

        public ShopifyServiceGraphQl(string storeName, string accessToken, string apiVersion)
        {
            _storeName = storeName;
            _accessToken = accessToken;
            _apiVersion = apiVersion;
            _graphqlEndpoint = $"https://{_storeName}.myshopify.com/admin/api/{_apiVersion}/graphql.json";
            //_graphqlEndpoint = "https://prueba-2-no-usar-demasiado.myshopify.com/admin/api/2024-10/graphql.json";
        }

        public async Task<string> CreateProductAsync(
            string productName,
            string productDescription,
            string productVendorName,
            string productTypeName,
            string productStatus)
        {

            var mutation = @"mutation CreateProduct($input: ProductInput!) { productCreate(input: $input) { product { id title } userErrors { field message } } }";

            var variablesAUsar = new
            {
                input = new
                {
                    title = productName,
                    vendor = productVendorName,
                    productType = productTypeName,
                    status = "ACTIVE"
                }
            };

            var payload = new
            {
                query = mutation,
                variables = variablesAUsar
            };

            try
            {
                var jsonRequest = JsonSerializer.Serialize(payload);
                using HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, _graphqlEndpoint);

                request.Headers.Add("X-Shopify-Access-Token", _accessToken);

                request.Content = new StringContent(JsonSerializer.Serialize(payload));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                return "Response" + responseBody;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }


        }
        public async Task<string> webhookSubscriptionRequest(string topico, string urlRecibir)
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
        public async Task<string> webhookUpdateRequest(string webhookId, string nuevoUrlRecibir)
        {
            var mutation = @"
        mutation webhookSubscriptionUpdate($id: ID!, $webhookSubscription: WebhookSubscriptionInput!) {
            webhookSubscriptionUpdate(id: $id, webhookSubscription: $webhookSubscription) {
                userErrors {
                    field
                    message
                }
                webhookSubscription {
                    id
                    topic
                    endpoint {
                        ... on WebhookHttpEndpoint {
                            callbackUrl
                        }
                    }
                }
            }
        }";


            var variableJson = new
            {
                id = webhookId,
                webhookSubscription = new
                {
                    callbackUrl = nuevoUrlRecibir
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
        public async Task<string> UpdateProduct(string productId,
                                                string title = null,
                                                string descriptionHtml = null,
                                                string handle = null,
                                                string productType = null,
                                                string vendor = null,
                                                List<string> tags = null,
                                                List<MetafieldInput> metafields = null,
                                                List<MediaInput> mediaInputs = null)
        {
            var mutation = @"
                mutation UpdateProduct($input: ProductInput!, $media: [CreateMediaInput!]) {
                    productUpdate(input: $input, media: $media) {
                        product {
                            id
                            title
                            descriptionHtml
                            handle
                            tags
                            productType
                            vendor
                            metafields(first: 3) {
                                edges {
                                    node {
                                        id
                                        namespace
                                        key
                                        value
                                    }
                                }
                            }
                            media(first: 10) {
                                nodes {
                                    alt
                                    mediaContentType
                                    preview {
                                        status
                                    }
                                }
                            }
                        }
                        userErrors {
                            field
                            message
                        }
                    }
                }";


            var variable = new
            {
                input = new
                {
                    id = productId,
                    title,
                    descriptionHtml,
                    handle,
                    productType,
                    vendor,
                    tags,
                    metafields = metafields?.Select(m => new
                    {
                        m.Namespace,
                        m.Key,
                        m.Type,
                        m.Value,
                        m.Id
                    }).ToList()
                },
                media = mediaInputs?.Select(m => new
                {
                    m.OriginalSource,
                    m.Alt,
                    m.MediaContentType
                }).ToList()
            };

            var payload = new
            {
                query = mutation,
                variables = variable
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
        public class MediaInput
        {
            public string OriginalSource { get; set; } // URL of the media
            public string Alt { get; set; } // Alt text for the media
            public string MediaContentType { get; set; } // Type of media ("IMAGE", "VIDEO", etc.)
        }
        public class MetafieldInput
        {
            public string Id { get; set; }
            public string Namespace { get; set; }
            public string Key { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
        }
    }
}
