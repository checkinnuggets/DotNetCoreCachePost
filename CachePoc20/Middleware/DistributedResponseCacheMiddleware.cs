using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CachePoc20.Middleware
{
    public static class ResponseCachingExtensions
    {
        public static IApplicationBuilder UseDistributedResponseCaching(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<DistributedResponseCachingMiddleware>();
        }
    }

    public class DistributedResponseCachingMiddleware : ResponseCachingMiddleware
    {
        public DistributedResponseCachingMiddleware(
            RequestDelegate next,
            IOptions<ResponseCachingOptions> options,
            ILoggerFactory loggerFactory,
            IResponseCachingPolicyProvider policyProvider,
            IResponseCache cache,
            IResponseCachingKeyProvider keyProvider)
            : base(next, options, loggerFactory, policyProvider, keyProvider)
        {
            var cacheFieldInfo = typeof(ResponseCachingMiddleware)
                .GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);

            cacheFieldInfo.SetValue(this, cache);
        }
    }
}
