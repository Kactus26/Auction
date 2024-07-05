using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AuctionGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private readonly HttpClient _httpClient;

        public DataController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("AuctionServer");
        }

        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData(CancellationToken cancellationToken)
        {
            string jwt = Request.Headers.Authorization!;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt[7..]);

            var response = await _httpClient.GetAsync($"Data/GetUserData", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserData(RegisterUserRequest userRequest, CancellationToken cancellationToken)
        {
            string jwt = Request.Headers.Authorization!;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt[7..]);

            var response = await _httpClient.PostAsJsonAsync($"Data/AddUser", userRequest, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return Ok(await response.Content.ReadAsStringAsync());
        }
    }
}
