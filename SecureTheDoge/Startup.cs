using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecureTheDoge.Data;

namespace SecureTheDoge
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath);
            builder.AddJsonFile("appsettings.json", false, true);
            _configRoot = builder.Build();

            _env = env;
        }
        
        IConfigurationRoot _configRoot;
        private IHostingEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configRoot);
            services.AddDbContext<PrisonContext>(ServiceLifetime.Scoped);
            services.AddScoped<IPrisonRepository, PrisonRepository>();

            services.AddTransient<PrisonDbInitializer>();
            services.AddHttpContextAccessor();

            services.AddAutoMapper();

            //services.AddCors(cfg =>
            //{
            //    cfg.AddPolicy("SomePolicy", bldr =>
            //    {
            //        bldr.AllowAnyHeader()
            //            .AllowAnyMethod()
            //            .WithOrigins("http://SomeName.com");
            //    });
            //    cfg.AddPolicy("AnyGet", bldr =>
            //    {
            //        bldr.AllowAnyHeader()
            //            .WithMethods("GET")
            //            .AllowAnyOrigin();
            //    });
            //});

            services.AddMvc(opt =>
            {
                //if (!_env.IsProduction())
                //{
                //    opt.SslPort = 5001;
                //}
                //opt.Filters.Add(new RequireHttpsAttribute());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            PrisonDbInitializer seeder)
        {
            loggerFactory.AddConsole(_configRoot.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            //app.UseCors();

            app.UseMvc(config =>
            {
                //config.MapRoute("MainAPIRoute", "api/{controller}/{action}");
            });

            seeder.Seed().Wait();
        }
    }
}
