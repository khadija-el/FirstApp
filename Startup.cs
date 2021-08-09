using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FistApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using Extensions;

namespace FistApi
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
                   services.AddCors(options =>
             {
                  options.AddPolicy("CorsPolicy", builder =>
               {
                   builder
                   // .WithOrigins ("http://localhost:4200")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
               });
              });

             services.AddControllers();
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "FistApi", Version = "v1" });
            // });

               services.AddDbContext<FistApiContext>(options =>
            {
                // options.UseSqlServer(Configuration.GetConnectionString("FirstApp"));
                // options.UseSqlServer(Configuration.GetConnectionString("db"));
                 options.UseSqlite(Configuration.GetConnectionString("sqlite"));
                // options.EnableSensitiveDataLogging();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FistApi v1"));
            }
             else
            {
                app.Use(async (context, next) =>
                {
                    await next();

                    if (context.Response.StatusCode == 404
                        && !Path.HasExtension(context.Request.Path.Value))
                    {
                        context.Request.Path = "/index.html";
                        await next();
                    }
                });
            }

            // app.UseHttpsRedirection();

            app.UseRouting();


            app.UseCors("CorsPolicy");

            // app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ErrorHandler>();

            var provider = new FileExtensionContentTypeProvider();
            // provider.Mappings.Add(".exe", "application/octect-stream");
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true, //allow unkown file types also to be served
                // DefaultContentType = "Whatver you want eg: plain/text" //content type to returned if fileType is not known.
                ContentTypeProvider = provider
            });

            // app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
