﻿using CommonDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("FindLot")]
        public async Task<IActionResult> GetLots(PaginationLotSearchDTO paginationDTO, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("Lots/FindLot", paginationDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("GetLots")]
        public async Task<IActionResult> GetLots(PaginationDTO paginationDTO, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("Lots/GetLots", paginationDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("CreateLot")]
        [Authorize]
        public async Task<IActionResult> CreateLot(CreateLotWithImageDTO lotWithImageDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Lots/CreateLot", lotWithImageDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("CloseLot")]
        [Authorize]
        public async Task<IActionResult> CloseLot(UserIdDTO lotIdDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Lots/CloseLot", lotIdDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("SendOffer")]
        [Authorize]
        public async Task<IActionResult> SendOffer(OfferPrice offerPrice, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Lots/SendOffer", offerPrice, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }


        [HttpGet("GetUserBalanceAndEmail")]
        [Authorize]
        public async Task<IActionResult> GetUserBalanceAndEmail(CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.GetAsync("Data/GetUserBalanceAndEmail", cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }


        [HttpPost("GetLotAndOffersInfo")]
        public async Task<IActionResult> GetLotOffersInfo(UserIdDTO userLotIdDTO, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("Lots/GetLotAndOffersInfo", userLotIdDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("GetLotSellerInfo")]
        public async Task<IActionResult> FindUser(UserIdDTO userLotIdDTO, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("Lots/GetLotSellerInfo", userLotIdDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("GetUserLots")]
        [Authorize]
        public async Task<IActionResult> GetUserLots(PaginationDTO paginationDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Lots/GetUserLots", paginationDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("UnblockUser")]
        [Authorize]
        public async Task<IActionResult> UnblockUser(UserIdDTO userIdDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Friends/UnblockUser", userIdDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("BlockUser")]
        [Authorize]
        public async Task<IActionResult> BlockUser(UserIdDTO userIdDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Friends/BlockUser", userIdDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("RemoveFriend")]
        [Authorize]
        public async Task<IActionResult> RemoveFriend(UserIdDTO userIdDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Friends/RemoveFriend", userIdDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("AddFriend")]
        [Authorize]
        public async Task<IActionResult> AddFriend(UserIdDTO userIdDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Friends/AddFriend", userIdDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("GetUsersFriendshipStatus")]
        [Authorize]
        public async Task<IActionResult> GetUsersFriendshipStatus(UserIdDTO userIdDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Friends/GetUsersFriendshipStatus", userIdDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("FindUserInvitations")]
        [Authorize]
        public async Task<IActionResult> FindUserInvitations(PaginationDTO paginationDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();
            var response = await _httpClient.PostAsJsonAsync("Friends/FindUserInvitations", paginationDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("FindUser")]
        public async Task<IActionResult> FindUser(PaginationUserSearchDTO userSearchDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Friends/FindUser", userSearchDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [Authorize]
        [HttpPost("GetUserFriends")]
        public async Task<IActionResult> GetUserFriends(PaginationDTO paginationDTO, CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.PostAsJsonAsync("Friends/GetUserFriends", paginationDTO, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("IsEmailConfirmed")]//Returns IsEmailConfirmed field
        public async Task<IActionResult> IsEmailConfirmed(UserIdDTO userId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("Data/IsEmailConfirmed", userId, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [Authorize]
        [HttpGet("EmailIsConfirmed")]//Changes IsEmailConfirmed field
        public async Task<IActionResult> EmailIsConfirmed(CancellationToken cancellationToken)
        {
            JWTIntoHeader();

            var response = await _httpClient.GetAsync("Data/EmailIsConfirmed", cancellationToken);

            if (!response.IsSuccessStatusCode)
                return BadRequest(await response.Content.ReadAsStringAsync());

            return Ok(await response.Content.ReadAsStringAsync());
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
            if (jwt == null)
                return;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt[7..]);
        }
    }
}
