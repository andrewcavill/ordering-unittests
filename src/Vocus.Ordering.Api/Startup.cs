using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vocus.Common.Middleware;
using Vocus.Ordering.Repositories;
using Vocus.Ordering.Repositories.Interfaces;
using Vocus.Ordering.Services;
using Vocus.Ordering.Services.Interfaces;

namespace Vocus.Ordering.Api
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
            VMiddlewareHelper.ConfigureServices(services, Configuration);

            services.AddSingleton<IBrandRepository, BrandRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();

            services.AddSingleton<IBrandService, BrandService>();
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		
		public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider) => VMiddlewareHelper.ConfigureMiddleware(app, serviceProvider);
		

    }
}
