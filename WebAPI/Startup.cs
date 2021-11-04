using DataLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceLayer.LocomotiveService;
using ServiceLayer.LocomotiveService.Concrete;
using ServiceLayer.ProductService;
using ServiceLayer.ProductService.Concrete;
using ServiceLayer.UserService;
using ServiceLayer.UserService.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Formatters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WebAPI
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
            #region Database
            services.AddDbContext<EShopContext>(options => options = new DbContextOptionsBuilder());
            #endregion

            #region Dependency injection
            services.AddScoped<ILocomotiveService, LocomotiveService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            #endregion

            #region Swagger
            services.AddSwaggerGen();
            #endregion

            #region WebApp hosting
            services.AddRazorPages();
            #endregion

            #region Formatters
            services.AddControllers().AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();

            //services.AddControllers(options =>
            //{
            //    options.OutputFormatters.Insert(0, new YamlOutputFormatter(new SerializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build()));
            //    options.InputFormatters.Insert(0, new YamlInputFormatter(new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build()));
            //})
            //.AddXmlSerializerFormatters()
            //.AddXmlDataContractSerializerFormatters();
            #endregion

            #region CORS
            services.AddCors();
            #endregion

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI();
            #endregion

            app.UseRouting();

            #region CORS
            app.UseCors(options => {
                options.AllowAnyMethod();
                //options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.SetIsOriginAllowed(origin => true);
                options.AllowCredentials();
            });
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region WebApp hosting
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");

            });
            #endregion
        }
    }
}