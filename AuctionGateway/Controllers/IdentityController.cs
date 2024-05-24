using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;

namespace AuctionGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        private readonly HttpClient _httpClient;

        public IdentityController(IHttpClientFactory httpClient) 
        {
            _httpClient = httpClient.CreateClient("IdentityServer");
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegistrationUser(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync($"User/RegisterUser", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return Ok(await response.Content.ReadAsStringAsync());
        }

    }
}
