using System;
using Microsoft.AspNetCore.Mvc;

namespace CachePoc11.Controllers
{
    [Route("api/[controller]")]
    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any)]  // duration in seconds
    public class ClockController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return DateTime.Now.ToString("U");
        }
    }
}
