using AutoMapper;
using Core;
using Core.Services;
using Core.Services.Implementations;
using Data.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace SmartProxyWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            // Configuration
            services.Configure<RedisSettings>(Configuration.GetSection(nameof(RedisSettings)));

            services.AddSingleton<IRedisSettings>(sp => sp.GetRequiredService<IOptions<RedisSettings>>().Value);

            // Services
            services
                .AddTransient<IMessageService, ProxyMessageService>()
                .AddTransient<IMessageCacheService, MessageCacheService>()
                .AddSingleton<IProxyHostService, ProxyHostService>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // AutoMapper
            services.AddAutoMapper(typeof(AutoMapping));

            // HttpClient
            services.AddHttpClient();

            // Redis
            var redisSettings = Configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.Host;
                options.InstanceName = "master";
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}