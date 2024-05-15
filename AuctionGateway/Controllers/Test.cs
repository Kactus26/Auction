using AuctionIdentity.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;

namespace AuctionGateway.Controllers
{
    public class Test : Controller
    {
        private readonly HttpClient _httpClient;

        public Test(IHttpClientFactory httpClient) 
        {
            _httpClient = httpClient.CreateClient("IdentityServer");
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegistrationUser(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync($"User/Test", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync(cancellationToken));
            }
            return Ok(response.Content.ReadAsStringAsync());
        }

    }
}
