using AuctionAPI.Data;
using AuctionAPI.Interfaces.PublisherSubscriber;
using AuctionAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace AuctionAPI
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
            services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                .Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddDbContextPool<BidContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AuctionAppConn")));
            services.AddDbContextPool<BidderContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AuctionAppConn")));
            services.AddDbContextPool<ItemContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AuctionAppConn")));
            services.AddDbContextPool<AutoBidContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AuctionAppConn")));
            services.AddScoped<IBidService, BidService>();
            services.AddScoped<IBidderService, BidderService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IPubSub, PublishSubscribeMiddleMan>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors( options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() );
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
