using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Net.Http.Headers;

namespace ShopifyAppSherable.Services
{
    public class ShopifyService
    {
        private readonly string _storeName;
        private readonly string _access_token;
        private readonly string _apiVersion;
        private string _BuildApiUrl = "";
        public ShopifyService(string store, string token, string apiVersion)
        {
            _access_token = token;
            _apiVersion = apiVersion;
            _storeName = store;
            _BuildApiUrl = $"https://{_storeName}.myshopify.com/admin/api/{_apiVersion}";
        }
        public async Task<string> PublicarMensajePOST(object message, string UrlComunicacion)
        {
            //Solo puede ser usado en POST
            try
            {
                using HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, UrlComunicacion);

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
        public async Task<string> MensajeGET(string UrlComunicacion)
        {
            //Solo puede ser usado en GET
            try
            {
                using HttpClient client = new HttpClient();


                client.DefaultRequestHeaders.Add("X-Shopify-Access-Token", _access_token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = await client.GetAsync(UrlComunicacion);
                string responseBody = await response.Content.ReadAsStringAsync();
                return "Response: " + responseBody;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<string> RegisterWebhook(string topico, string urlObjetivo)
        {
            string url = $"{_BuildApiUrl}/webhooks.json";

            var payload = new
            {
                webhook = new
                {
                    topic = topico,
                    address = urlObjetivo,
                    format = "json"
                }
            };
            return await PublicarMensajePOST(payload, url);
        }
        public async Task<string> createNewProductSingle(
            string productName, string productDescription,
            string productVendorName, string productType,
            string productStatus
            )
        {
            if (productStatus != "active" && productStatus != "archived" && productStatus != "draft")
            {
                return "El product status solo puede ser active, archived, o draft";
            }
            string url = $"{_BuildApiUrl}/products.json";
            var payload = new
            {
                product = new
                {
                    title = productName,
                    bodyHtml = productDescription,
                    vendor = productVendorName,
                    product_type = productType,
                    status = productStatus
                }
            };
            return await PublicarMensajePOST(payload, url);
        }
        public async Task<string> GetListOfProducts()
        {
            //Este servicio solicita la lista de todos los productos existentes
            var url = $"{_BuildApiUrl}/products.json";
            return await MensajeGET(url);
        }

        public async Task<string> GetListOfProductsFromIDs(string idListString)
        {
            //Este servicio solicita la lista de productos con los IDs especificados
            //Recibe los IDs como un string en el que cada ID está separado por una coma
            
            //elimina comas y espacios
            var idArray = idListString.Split(',')
                               .Select(id => id.Trim())
                               .Where(id => !string.IsNullOrEmpty(id))
                               .ToArray();
            if (idArray.Length == 0)
            {
                return "No hay IDs presentes.";
            }

            var formattedIds = string.Join(",", idArray);

            var url = $"{_BuildApiUrl}/products.json?ids={Uri.EscapeDataString(formattedIds)}";

            return await MensajeGET(url);
        }
    }
}
