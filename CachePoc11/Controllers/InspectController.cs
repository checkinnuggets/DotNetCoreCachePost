using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching.Internal;

namespace CachePoc11.Controllers
{
    [Route("api/[controller]")]
    public class InspectController : Controller
    {
        private readonly IResponseCache _responseCache;

        public InspectController(IResponseCache responseCache)
        {
            _responseCache = responseCache;
        }

        [HttpGet]
        public string Get()
        {
            // Here, in 1.1 it shows us the name of the type.  This defaults to 'Microsoft.AspNetCore.ResponseCaching.Internal.MemoryResponseCache'.
            // This comes from the call to AddResponseCaching in ConfigureServices, but is overridden by own own implementation
            // if we provide one.

            var responseCacheType = _responseCache.GetType().ToString();
            return $"ResponseCache is of type '{responseCacheType}'";           
        }
    }
}