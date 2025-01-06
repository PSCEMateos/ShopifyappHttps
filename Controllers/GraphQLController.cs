/*using ShopifyAppSherable.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace ShopifyAppSherable.Controllers
{
    public class GraphQLController : ControllerBase
    {
        private readonly GraphQLProductService _productService;
        private static List<string> _receivedMessages = new List<string>();
        private readonly string _shopifyApiKey = "";
        private readonly string _webhookToken = "";
        private readonly string _shopifyAccessToken = "";
        private readonly string _shopifyApiKey = "";
        public GraphQLController() 
        {
            _productService = new GraphQLProductService("prueba-2-no-usar-demasiado", _shopifyAccessToken, "2024-10");
        }
        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProductGraphQl(
            [FromQuery] string productName,
            [FromQuery] string productDescription,
            [FromQuery] string productVendorName,
            [FromQuery] string productType,
            [FromQuery] string productStatus)
        {
            _receivedMessages.Add("Start");

            if (productStatus != "ACTIVE" && productStatus != "ARCHIVED" && productStatus != "DRAFT")
            {
                return BadRequest("Product status must be ACTIVE, ARCHIVED, or DRAFT.");
            }
            _receivedMessages.Add("Starting");
            string response = await _productService.CreateProductAsync(
                productName,
                productDescription,
                productVendorName,
                productType,
                productStatus);

            _receivedMessages.Add(response);

            return Ok(response);
        }

        [HttpGet("messages")]
        public IActionResult GetMessages()
        {
            return Ok(_receivedMessages);
        }
    }
}
*/