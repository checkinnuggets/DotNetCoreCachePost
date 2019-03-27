using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching.Internal;

namespace CachePoc20.Controllers
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
            // Here, in 2.0 it blows up creating the instance of this controller
            //  - because there is no implementation of ICacheResponse registered
            //  - and if there was, the response caching middleware doesn't use it anyway.

            // if we provide our own implementation, it's name will come through here, but
            // since the constructor has been removed from the middleware, it isn't used.

            var responseCacheType = _responseCache.GetType().ToString();
            return $"ResponseCache is of type '{responseCacheType}'";           
        }
    }
}