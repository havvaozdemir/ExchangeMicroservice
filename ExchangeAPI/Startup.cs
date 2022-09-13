using ExchangeAPI.Models;
using Microsoft.EntityFrameworkCore;
using ExchangeAPI.Services;
using ExchangeAPI.Services.Handlers;
using Microsoft.Extensions.Caching.Memory;
using ExchangeAPI.Containers;
using AspNetCoreRateLimit;

namespace ExchangeAPI
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
            services.AddMemoryCache();
            services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));
            services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddHttpContextAccessor();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpClient("exchangeRatesApi", c =>
            {
                c.BaseAddress = new Uri(Configuration["ExchangeRatesApiBaseAddress"]);
            });
            services.AddDbContext<Exchange_DBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("constring"));
            });
            services.AddSingleton<IHttpClient, ExchangeRatesHttpClient>();

            services.AddScoped<IExchangeRatesService>(
                x =>
                new ExchangeRatesService(x.GetRequiredService<IMemoryCache>(),
                            x.GetRequiredService<IHttpClient>(),
                            Configuration["ExchangeRatesApiKey"],
                            int.Parse(Configuration["CacheExpirationMinutes"])));

            services.AddScoped<IExchangeRateContainer, ExchangeRateContainer>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }
            app.UseClientRateLimiting();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            DatabaseManagementContainer.Migrate(app);
        }
    }
}