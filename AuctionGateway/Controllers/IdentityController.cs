using Azure.Core;
using CommonDTO;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("SendEmail")]
        public async Task<IActionResult> SendEmail(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"User/SendEmail", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return Ok(await response.Content.ReadAsStringAsync());
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

        [HttpPost("Authorization")]
        public async Task<IActionResult> Authorization(AuthUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync($"User/AuthorizeUser", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return Ok(await response.Content.ReadAsStringAsync());
        }


        [HttpPost("TestAuthGateway")]
        [Authorize]//Method that cheks is user authorized and token is actual
        public IActionResult TestAuthGateway()
        {
            return Ok();
        }
    }
}
