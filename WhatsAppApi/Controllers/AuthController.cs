using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using WhatsAppApi.DTO;
using WhatsAppApi.FirstMessageTemplate;
using WhatsAppApi.Services;

namespace WhatsAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WhatsAppSettings _settings;

        public AuthController(IOptions<WhatsAppSettings> settings)
        {
            _settings = settings.Value;
        }
        [HttpPost]
        public async Task<IActionResult> SendWelcomeMessage(SendMessageDTO dto)
        {
            var language = Request.Headers["language"].ToString();
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _settings.Token);

            var body = new WhatsAppRequest
            {
                to = dto.Mobile,
                template = new Template
                {
                    name = "hello_world",
                    language = new Language
                    {
                        code = language
                    }
                }
            };

            HttpResponseMessage response =
                        await httpClient.PostAsJsonAsync(new Uri(_settings.ApiUrl), body);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong !!");
            }
            return Ok();
        }
    }
}
