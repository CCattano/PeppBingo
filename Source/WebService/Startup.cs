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
using Pepp.Web.Apps.Bingo.WebService.Controllers.Translators;
using Pepp.Web.Apps.Bingo.WebService.Middleware;
using Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation;
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
            services.AddScoped<IUserAdapter, UserAdapter>();
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
                typeof(Entity_BusinessEntity),
                typeof(BusinessModels_BusinessEntities)
            );

            services.AddOpenApiDocument(cfg =>
            {
                cfg.SchemaType = NJsonSchema.SchemaType.OpenApi3;
                cfg.Title = "Tandem.Web.Apps.Trivia";
            });
            #endregion

            #region MISC
            services.SetTokenValidationPathsToExclude();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.MapWhen(
                // When request path is /status/isalive.
                path => path.Request.Path.Value?.ToLower() == "/status/isalive",
                // Return this message.
                builder => builder.Run(async context => await context.Response.WriteAsync($"PeppBingo SPA server is currently running."))
            );

            /*
             * Order of operations is very important here
             * The request comes in and flows through these middleware in the order they're registered in
             * 
             * We want the request to go into our exception handler MW first
             *      This invokes all other logic inside a try catch
             * Then we want the request to go into our token validator MW
             *      This will ensure our request contains a valid JWT if required
             *      That way we shut down the request ASAP
             * Then we flow into our cache hydration MW
             *      This will ensure our cache is current for whatever work we're about to do
  
             *      
             * When an exception occurs or a response is returned
             * That response, whether an error or data bubbles back up our MW stack
             * So w/ an exception our exception MW gets the error last and sets our response
             * 
             * When the request is coming in it's critical that we flow in the following order
             *      Exception Handler
             *      Token Validation
             *      Cache Hydration
             * 
             * This is the optimal input flow, and guarantees our exception handler
             * which writes our response in the event of an exception, receives any 
             * thrown exception before our response leaves this server
             */
            app.UseExceptionHandlerMiddleware();
            app.UseTokenValidationMiddleware();
            app.UseCacheHydrationMiddleware();

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
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
