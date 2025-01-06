/*
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShopifyAppSherable.Services
{
    public class CreateNewProduct
    {
        private readonly string _StoreName;
        private readonly string _access_token;
        private readonly string _apiVersion;
        private string _BuildApiUrl = "";
        public CreateNewProduct(string store, string token, string apiVersion)
        {
            _access_token = token;
            _apiVersion = apiVersion;
            _StoreName = store;
            _BuildApiUrl = $"https://{_StoreName}.myshopify.com/admin/api/{_apiVersion}";
        }
        public async Task<string> PublicarProducto(object message)
        {
            //Solo puede ser usado en POST
            _BuildApiUrl = $"{_BuildApiUrl}/products.json";
            try
            {
                using HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, _BuildApiUrl);

                request.Headers.Add("X-Shopify-Access-Token", _access_token);

                request.Content = new StringContent(JsonSerializer.Serialize(message));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();
                return "Response" + responseBody;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<string> createNewProductSingle(
            string productName, 
            string productDescription, 
            string productVendorName, 
            string productType,
            string productStatus
            )
        {
            if (productStatus != "active" && productStatus != "archived" && productStatus != "draft")
            {
                return "El product status solo puede ser active, archived, o draft";
            }
            var message = new
            {

                product = new {
                    title = productName,
                    bodyHtml = productDescription,
                    vendor = productVendorName,
                    product_type = productType,
                    status = productStatus
                }
            };
            return await PublicarProducto(message);

        }
    }
}
*/