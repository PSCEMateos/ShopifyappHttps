
using ShopifyAppSherable.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ShopifyAppSherable.Controllers
{
    public class CrearWebhook: ControllerBase
    {
        private readonly string _shopifyApiKey1 = "";
        private readonly string _webhookToken = "";
        private readonly string _shopifyAccessToken = "";
        private readonly string _shopifyApiKey = "";

        private readonly string _miUrlRecibir = "https://iyd.azurewebsites.net/api/webhook/recive";

        private readonly ShopifyServiceGraphQl _shopifyServiceGraphQl;
        private static List<string> _receivedMessages = new List<string>();

        public CrearWebhook() 
        {
            _shopifyServiceGraphQl = new ShopifyServiceGraphQl("prueba-2-no-usar-demasiado", _shopifyAccessToken, "2024-10");

        }

        [HttpPost("api/crearWebhook")]
        public async Task<IActionResult> CrearWebhookConTopico([FromQuery] string webhookTopic)
        {
            _receivedMessages.Add("Start");
            if (string.IsNullOrEmpty(webhookTopic))
            {
                _receivedMessages.Add("Error: El tópico no puede ser nulo o vacío.");
                throw new ArgumentException("El tópico no puede ser nulo o vacío.", nameof(webhookTopic));
            }
            webhookTopic = webhookTopic.Trim().Replace(" ", "");
            if (!Regex.IsMatch(webhookTopic, @"^[a-zA-Z]+(/[a-zA-Z]+|_[a-zA-Z]+)$"))
            {
                throw new FormatException("El tópico no tiene el formato válido 'x/y' o 'x_y'.");
            }
            webhookTopic = webhookTopic.Replace("/", "_");
            webhookTopic = webhookTopic.ToUpperInvariant();

            var respuesta = await _shopifyServiceGraphQl.webhookSubscriptionRequest(webhookTopic, _miUrlRecibir);

            _receivedMessages.Add(respuesta);
            return Ok();
        }
        [HttpPost("api/updateWebhook")]
        public async Task<IActionResult> UpdateWebhook([FromQuery] string webhookID)
        {
            _receivedMessages.Add("Start");
            var result = await _shopifyServiceGraphQl.webhookUpdateRequest(webhookID, _miUrlRecibir);
            _receivedMessages.Add($"Update Result: {result}");
            return Ok(result);
        }

        private IActionResult ok(string v)
        {
            throw new NotImplementedException();
        }

        [HttpGet("api/crearWebhook/messages")]
        public dynamic GetMessages()
        {
            // Return the list of received messages as the response 
            return _receivedMessages;
        }
    }
}
