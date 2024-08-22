using Azure.Core;
using CommonDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
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

        [HttpPost("UserIdPasswordRecovery")]
        public async Task<IActionResult> UserIdPasswordRecovery(LoginDTO login, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync($"User/UserIdPasswordRecovery", login, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(EmailDTO email, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync($"User/SendEmail", email, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegistrationUser(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync($"User/RegisterUser", request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("Authorization")]
        public async Task<IActionResult> Authorization(AuthUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync($"User/AuthorizeUser", request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

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
