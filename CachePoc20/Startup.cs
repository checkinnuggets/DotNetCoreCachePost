using CachePoc20.Middleware;
using CachePoc20.ResponseCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CachePoc20
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Response Caching - 2.0 registers IResponseCachingPolicyProvider and IResponseCachingKeyProvider
            services.AddResponseCaching();

            // Distributed Cache
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "SampleInstance";
            });

            // Here, we can add our own implementation of IResponseCache, but it does not get injected into the ResponseCache middleware
            services.AddSingleton<IResponseCache, DistributedResponseCache>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Response Caching - adds response caching middleware
            //app.UseResponseCaching();
            app.UseDistributedResponseCaching();

            app.UseMvc();
        }
    }
}
