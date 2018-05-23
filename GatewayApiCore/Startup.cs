using System;
using DashboardActor.Interfaces;
using GatewayApiCore.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace GatewayApiCore
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
            services.AddMvc();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<DashboardHub>("/dashboard");
            });

            // Subscribe to dashboard events so we can forward them to the client using the SignalR DashboardHub.
            var dashboardHub = serviceProvider.GetService<IHubContext<DashboardHub>>();
            //
            var proxy = ActorProxy.Create<IDashboardActor>(new ActorId("demo"));
            proxy.SubscribeAsync<IDashboardEvents>(new DashboardEventHandler(dashboardHub))
                .GetAwaiter().GetResult();
        }
    }
}
