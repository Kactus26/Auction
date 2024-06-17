using Microsoft.AspNetCore.Mvc;

namespace AuctionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData()
        {
            

            return Ok(test);
        }
    }
}
