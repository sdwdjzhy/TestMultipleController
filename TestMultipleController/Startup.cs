using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControllerLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestMultipleController
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ControllerNameSelector.IsValidForRequest = context =>
            {
                var controllerActionDescriptor = (ControllerActionDescriptor) context.CurrentCandidate.Action;
                var controllerNamespace = controllerActionDescriptor.MethodInfo.DeclaringType?.Namespace;
                return controllerNamespace == null || !controllerNamespace.Equals("ControllerLib.Controllers", StringComparison.CurrentCultureIgnoreCase);
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(
                endpoints =>
                {
                    //endpoints.MapGet("/", async context =>
                    //{
                    //    await context.Response.WriteAsync("Hello World!");
                    //});
                    var dataTokens = new RouteValueDictionary();

                    var ns = new[] { "TestMultipleController.Controllers" };

                    dataTokens["Namespaces"] = ns;

                    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}", defaults: null, constraints: null, dataTokens: dataTokens);
                });
        }
    }
}