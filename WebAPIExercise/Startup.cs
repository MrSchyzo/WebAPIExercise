using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAPIExercise.Data;
using WebAPIExercise.Data.UnitOfWork;
using WebAPIExercise.Mapping;
using WebAPIExercise.Services;

namespace WebAPIExercise
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
            services.AddDbContext<ShopContext>(opts => opts.UseSqlite(@"Data Source=Shop.db;"));

            services.AddSingleton<ICompanyTotalConverter, DictionaryCompanyTotalConverter>();

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddScoped<IProductService, ShopProductService>();
            services.AddScoped<IOrderService, ShopOrderService>();

            services.AddScoped<IProductRepository, ShopProductRepository>();
            services.AddScoped<IOrderRepository, ShopOrderRepository>();
            services.AddScoped<ShopUnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
