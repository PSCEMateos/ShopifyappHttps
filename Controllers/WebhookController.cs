using Microsoft.AspNetCore.Mvc;
using ShopifyAppSherable.Models;
using ShopifyAppSherable.Services;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ShopifyAppSherable.Controllers
{
    public class WebhookController : ControllerBase
    {
        private readonly string _shopifyApiKey1 = "";
        private readonly string _webhookToken = "";
        private readonly string _shopifyAccessToken = "";
        private readonly string _shopifyApiKey = "";
        private readonly WebhookVerification _webhookVerification;
        private static readonly ConcurrentQueue<WebhookModel> _webhookQueue = new ConcurrentQueue<WebhookModel>();
        private readonly ShopifyServiceGraphQl _shopifyServiceGraphQl;
        private static List<string> _receivedMessages = new List<string>();

        public WebhookController()
        {
            _webhookVerification = new WebhookVerification(_webhookToken, "prueba-2-no-usar-demasiado", _shopifyAccessToken, "2024-10");
            _shopifyServiceGraphQl = new ShopifyServiceGraphQl("prueba-2-no-usar-demasiado", _shopifyAccessToken, "2024-10");
        }



        [HttpPost("api/webhook/recive")]
        public async Task<IActionResult> ReceiveWebhook([FromHeader(Name = "X-Shopify-Hmac-Sha256")] string shopifyHmac)
        {
            string requestBody;

            _receivedMessages.Add("Message recived");
            _receivedMessages.Add($"X-Shopify-Hmac-Sha256: {shopifyHmac}");

            using (var reader = new StreamReader(Request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            _receivedMessages.Add("StreamReader Processed");

            _receivedMessages.Add($"Serialized Request body: {requestBody}");

            if (!_webhookVerification.VerifySignature(requestBody, shopifyHmac))
            {
                _receivedMessages.Add("Invalid webhook signature");
                return Unauthorized("Invalid webhook signature");
            }
            _receivedMessages.Add("webhook signature Válido");

            try
            {
                var payload = JsonSerializer.Deserialize<WebhookModel>(requestBody);
                if (payload == null)
                {
                    _receivedMessages.Add("Invalid payload");
                    return BadRequest("Invalid payload");
                }

                _webhookQueue.Enqueue(payload);

                _receivedMessages.Add("Message recived");
                _receivedMessages.Add($"Topic: {payload.Topic}");
                _receivedMessages.Add($"ID: {payload.id}");
                _receivedMessages.Add($"Data: {payload.Data}");
                _receivedMessages.Add($"Created At: {payload.created_at}");
            }
            catch (NotSupportedException ex)
            {
                _receivedMessages.Add("Topic not supported");
                return BadRequest("Topic not supported");
            }
            catch (Exception ex) 
            {
                _receivedMessages.Add("payload processing failed");
                return BadRequest("payload processing failed");
            }

            _receivedMessages.Add("Webhook recibido con éxito");
            return Ok("Webhook received and queued");
        }
        [HttpGet("api/webhook/process-queue")]
        public IActionResult ProcessWebhookQueue()
        {
            while (_webhookQueue.TryDequeue(out var payload))
            {
                // Simulate processing (e.g., store in database, perform business logic)
                Console.WriteLine($"Processed webhook: {payload.id}, Topic: {payload.Topic}");
                _receivedMessages.Add("ProcessWebhookQueue:");
                _receivedMessages.Add($"Processed webhook: {payload.id}, Topic: {payload.Topic}");
            }

            return Ok("Queue processed");
        }
        [HttpPost("api/products/update")]
        public async Task<IActionResult> UpdateProduct()
        {
            string resultado = await _shopifyServiceGraphQl.UpdateProduct(productId: "gid://shopify/Product/8654783447282",
                                                                          title: "Abrigo",
                                                                          descriptionHtml: "<p>This is the updated description.</p>",
                                                                          productType: "Abrigo de tela",
                                                                          tags: new List<string> { "new-arrival", "featured", "Abrigo" },
                                                                          vendor: "Yours Trully");
            _receivedMessages.Add($"Update result: {resultado}");
            return Ok(resultado);
        }

        //Recabamos los Mensajes VIA GET DENTRO DE LA RUTA webhook 
        [HttpGet("messages")]
        public dynamic GetMessages()
        {
            // Return the list of received messages as the response 
            return _receivedMessages;
        }
    }
}


/*Verson 1: no funciona el request body
using Microsoft.AspNetCore.Mvc;
using ShopifyappHttps.Models;
using ShopifyappHttps.Services;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ShopifyappHttps.Controllers
{
    public class WebhookController :  ControllerBase
    {
        private readonly string _shopifySecretApiKey = "e81b76b3d95244d2f81787dd0103c3a8";
        private readonly string _webhookSecretToken = "4a6687557e21e81afeffac4a1351d6f482b6108759860ec4108366c488a2cca2";
        private readonly string _shopifyAccessToken = "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";
        private readonly string _shopifyApiKey = "be2e3ac9e79db2cb2e671b574a4f4dcd";
        private readonly WebhookVerification _webhookVerification;
        private static readonly ConcurrentQueue<WebhookModel> _webhookQueue = new ConcurrentQueue<WebhookModel>();
        private readonly ShopifyServiceGraphQl _shopifyServiceGraphQl;
        private static List<string> _receivedMessages = new List<string>();

        public WebhookController()
        {
            _webhookVerification = new WebhookVerification(_webhookSecretToken, "prueba-2-no-usar-demasiado", _shopifyAccessToken, "2024-10");
            _shopifyServiceGraphQl = new ShopifyServiceGraphQl("prueba-2-no-usar-demasiado", _shopifyAccessToken, "2024-10");
        }
        


        [HttpPost("api/webhook/recive")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WebhookModel payload, [FromHeader(Name = "X-Shopify-Hmac-Sha256")] string shopifyHmac)
        {
            string requestBody;

            _receivedMessages.Add("Message recived");
            _receivedMessages.Add($"Topic: {payload.Topic}");
            _receivedMessages.Add($"ID: {payload.Id}");
            _receivedMessages.Add($"Data: {payload.Data}");
            _receivedMessages.Add($"Created At: {payload.CreatedAt}");
            _receivedMessages.Add($"X-Shopify-Hmac-Sha256: {shopifyHmac}");

            using (var reader = new StreamReader(Request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            //var headers = Request.Headers;
            //_receivedMessages.Add($"Headers: {string.Join(", ", headers)}");

            _receivedMessages.Add("Try");

            _receivedMessages.Add($"Serialized Request body: {requestBody}");

            //_receivedMessages.Add(requestBody);

            if (!_webhookVerification.VerifySignature(requestBody, shopifyHmac))
            {
                _receivedMessages.Add("Invalid webhook signature");
                return Unauthorized("Invalid webhook signature");
            }
            _receivedMessages.Add("webhook signature Válido");

            _webhookQueue.Enqueue(payload);


            _receivedMessages.Add("Recibido con éxito");
            return Ok("Webhook received and queued");
        }
        [HttpGet("api/webhook/process-queue")]
        public IActionResult ProcessWebhookQueue()
        {
            while (_webhookQueue.TryDequeue(out var payload))
            {
                // Simulate processing (e.g., store in database, perform business logic)
                Console.WriteLine($"Processed webhook: {payload.Id}, Topic: {payload.Topic}");
                _receivedMessages.Add("ProcessWebhookQueue:");
                _receivedMessages.Add($"Processed webhook: {payload.Id}, Topic: {payload.Topic}");
            }

            return Ok("Queue processed");
        }
        [HttpPost("api/products/update")]
        public async Task<IActionResult> UpdateProduct()
        {
            string resultado = await _shopifyServiceGraphQl.UpdateProduct(productId: "gid://shopify/Product/8654783447282",
                                                                          title: "Abrigo",
                                                                          descriptionHtml: "<p>This is the updated description.</p>",
                                                                          productType: "Abrigo de tela",
                                                                          tags: new List<string> { "new-arrival", "featured", "Abrigo" },
                                                                          vendor: "Yours Trully");
            _receivedMessages.Add($"Update result: {resultado}");
            return Ok(resultado);
        }

        //Recabamos los Mensajes VIA GET DENTRO DE LA RUTA webhook 
        [HttpGet("messages")]
        public dynamic GetMessages()
        {
            // Return the list of received messages as the response 
            return _receivedMessages;
        }
    }
}*/