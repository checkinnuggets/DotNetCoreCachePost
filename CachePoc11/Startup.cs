using CachePoc11.ResponseCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CachePoc11
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
                1.1 requires the following packages...
                    Microsoft.AspNetCore.ResponseCaching
                    Microsoft.Extensions.Caching.Redis 
            */

            // Response Caching - 1.1 registers IResponseCachingPolicyProvider, IResponseCachingKeyProvider AND IResponseCache
            services.AddResponseCaching();

            // DistributedCache
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "SampleInstance";
            });

            // Here, we can add our own implementation of IResponseCache, and it will be injected into the ResponseCache middleware
            services.AddSingleton<IResponseCache, DistributedResponseCache>();

            // Add framework services.
            services.AddMvc();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Response Caching - adds response caching middleware
            app.UseResponseCaching();

            app.UseMvc();
        }
    }
}
