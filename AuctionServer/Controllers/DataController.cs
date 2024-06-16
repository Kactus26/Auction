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
            Obj test = new Obj { Name = "sdsd", Value = "dsdsds"};

            return Ok(test);
        }
    }
}
class Obj
{    public string Name { get; set; }
    public string Value { get; set; }
}