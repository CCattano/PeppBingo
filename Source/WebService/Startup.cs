using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.Adapters.Translators;
using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Facades;
using Pepp.Web.Apps.Bingo.Facades.Translators;
using Pepp.Web.Apps.Bingo.Infrastructure.Caches;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch;
using Pepp.Web.Apps.Bingo.Managers;
using Pepp.Web.Apps.Bingo.WebService.Middleware;
using ConnStrings =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.AppSettings.ConnStrings;
namespace Pepp.Web.Apps.Bingo.WebService
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
            #region FRAMEWORK
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddControllers();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            #endregion

            #region ADAPTERS
            services.AddScoped<ITwitchAdapter, TwitchAdapter>();
            #endregion

            #region MANAGERS
            services.AddScoped<ITokenManager, TokenManager>();
            #endregion

            #region FACADES
            services.AddScoped<ITwitchFacade, TwitchFacade>();
            services.AddScoped<IUserFacade, UserFacade>();
            #endregion

            #region CACHES
            services.AddScoped<ITwitchCache, TwitchCache>();
            services.AddScoped<ITokenCache, TokenCache>();
            #endregion

            #region SERVICES
            services.AddPracticeAPIDataService(Configuration.GetConnectionString(ConnStrings.PeppBingo));
            #endregion

            #region CLIENTS
            services.AddTwitchClient();
            #endregion

            #region EXTERNAL
            services.AddAutoMapper(
                typeof(BusinessEntity_TwitchClient),
                typeof(Entity_BusinessEntity)
            );

            services.AddOpenApiDocument(cfg =>
            {
                cfg.SchemaType = NJsonSchema.SchemaType.OpenApi3;
                cfg.Title = "Tandem.Web.Apps.Trivia";
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCacheHydrationMiddleware();
            app.UseExceptionHandlerMiddleware();
            app.MapWhen(
                // When request path is /status/isalive.
                path => path.Request.Path.Value?.ToLower() == "/status/isalive",
                // Return this message.
                builder => builder.Run(async context => await context.Response.WriteAsync($"PeppBingo SPA server is currently running."))
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });

            app.UseOpenApi(); //Generates OpenAPI-compliant schema for API
            app.UseSwaggerUi3(); //Generates UI that parses generated schema

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
