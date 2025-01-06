using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopifyAppSherable.Services
{
    public class GraphQLProductService
    {
        private readonly string _storeName;
        private readonly string _accessToken;
        private readonly string _apiVersion;
        private readonly string _graphqlEndpoint;
        private readonly HttpClient _httpClient;

        public GraphQLProductService(string storeName, string accessToken, string apiVersion)
        {
            _storeName = storeName;
            _accessToken = accessToken;
            _apiVersion = apiVersion;
            //_graphqlEndpoint = $"https://{_storeName}.myshopify.com/admin/api/{_apiVersion}/graphql.json";
            _graphqlEndpoint = "https://prueba-2-no-usar-demasiado.myshopify.com/admin/api/2024-10/graphql.json";
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
    }
}
