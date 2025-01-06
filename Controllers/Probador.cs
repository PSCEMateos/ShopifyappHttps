/*
using ShopifyAppSherable.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyAppSherable.Controllers
{
    public class Probador : ControllerBase
    {
        private readonly string _shopifyApiKey = "";
        private readonly string _webhookToken = "";
        private readonly string _shopifyAccessToken = "";
        private readonly string _shopifyApiKey = "";
        private static List<string> _receivedMessages = new List<string>();

        private readonly WebhookVerification webhookVerification;
        //private readonly ShopifyService _shopifyService;

        public Probador()
        {
            //_shopifyService = new ShopifyService("prueba-2-no-usar-demasiado", _shopifyAccessToken, "2024-10");
            webhookVerification = new WebhookVerification(_webhookSecretToken, "prueba-2-no-usar-demasiado", _shopifyAccessToken, "2024-10");
        }

        [HttpPost("api/Probador/LeerListaProductosEspecíficos")]
        public async Task<IActionResult> CrearProductoSolo()
        {
            //string lista_de_productos_id = "8654783447282, 47156432371954, 8651389468914, 8651389829362";
            _receivedMessages.Add("Start");
            var respuesta = await webhookVerification.SubscribeToWebhookAsync("CARTS_UPDATE", "https://iyd.azurewebsites.net/swagger/index.html/api/webhook/recive");
            //var respuesta = await _shopifyService.GetListOfProductsFromIDs(lista_de_productos_id);
            _receivedMessages.Add(respuesta);
            return Ok();
        }

        [HttpGet("messages")]
        public dynamic GetMessages()
        {
            // Return the list of received messages as the response 
            return _receivedMessages;
        }
    }
}
*/