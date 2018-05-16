using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Launcher
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
            services.AddSession(options =>
            {
                options.Cookie.Name = "SimpleDockerUI.sid";
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(10);                
            });
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SimpleDockerUI API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //支持Session
            app.UseSession();

            //支持视图登录控制
            app.UseMiddleware<Launcher.Middleware.ViewLoginMiddleware>();

            //支持静态文件
            app.UseStaticFiles();

            //支持API登录控制
            app.UseMiddleware<Launcher.Middleware.ApiLoginMiddleware>();

            app.UseMvc();
            var swaggerEndpoint = "";
            app.UseSwagger(option =>
            {
                swaggerEndpoint = option.RouteTemplate;
                swaggerEndpoint = "/" + swaggerEndpoint.Replace("{documentName}", "v1");
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerEndpoint, "SimpleDockerUI API V1");
            });
            app.Run(async (context) =>
            {
                if (context.Request.Path == "/")
                {
                    var rep = context.Response;
                    rep.ContentType = "text/html; charset=UTF-8";
                    await rep.WriteAsync(File.ReadAllText("wwwroot/View/index.html"));
                    return;
                }
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 NOT FOUND");
            });
        }
    }
}
