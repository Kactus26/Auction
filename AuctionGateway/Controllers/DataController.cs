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

        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData(CancellationToken cancellationToken)
        {
            string jwt = Request.Headers.Authorization!;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt[7..]);

            var response = await _httpClient.GetAsync($"Data/GetUserData", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

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

        [HttpPost("UpdateUserData")]
        [Authorize]
        public async Task<IActionResult> UpdateUserData(ChangedDataDTO newData, CancellationToken cancellationToken)
        {
            string jwt = Request.Headers.Authorization!;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt[7..]);

            var response = await _httpClient.PostAsJsonAsync($"Data/UpdateUserData", newData, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("UploadImage")]
        [Authorize]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile file)
        {
            string jwt = Request.Headers.Authorization!;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt[7..]);

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var content = new MultipartFormDataContent();
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                var fileContent = new ByteArrayContent(fileBytes);

                content.Add(fileContent, "file", file.FileName);

                var response = await _httpClient.PostAsync("Data/UploadImage", content);
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);  
            }
        }
    }
}
