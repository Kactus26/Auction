using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
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

        [Authorize]
        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData(CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.GetAsync($"Data/GetUserData", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserData(RegisterUserRequest userRequest, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync($"Data/AddUser", userRequest, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [Authorize]
        [HttpPost("UpdateUserData")]
        public async Task<IActionResult> UpdateUserData(UserDataWithImageDTO newData, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync($"Data/UpdateUserData", newData, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return Ok(await response.Content.ReadAsStringAsync());
        }

        private void JWTIntoHeader()
        {
            string jwt = Request.Headers.Authorization!;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt[7..]);
        }
    }
}
