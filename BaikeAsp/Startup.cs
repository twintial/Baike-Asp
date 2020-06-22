using AutoMapper;
using BaikeAsp.Dao;
using BaikeAsp.Dao.Impl;
using BaikeAsp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace BaikeAsp
{
    public class Startup
    {
        public static readonly ILoggerFactory efLogger = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole();
        });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache().AddSession();
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<BaikeContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("BaikeDatabase")).UseLoggerFactory(efLogger);
            });
            services.AddCors(options => options.AddPolicy("cors", p => p.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

            services.AddScoped<IUserReposity, UserReposity>();
            services.AddScoped<IUserInfoReposity, UserInfoReposity>();
            services.AddScoped<ICollectionReposity, CollectionReposity>();
            services.AddScoped<IFavouriteReposity, FavouriteReposity>();
            services.AddScoped<IInteractiveVideoReposity, InteractiveVideoReposity>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        // ÔÝÊ±ÕâÑùÐ´
                        await context.Response.WriteAsync("Server Error");
                    });
                });
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                // ÅäÖÃ¾²Ì¬Ä¿Â¼
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "resources")),
                RequestPath = "",
            });

            app.UseSession();

            app.UseCors("cors");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
