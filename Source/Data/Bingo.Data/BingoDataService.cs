using Microsoft.Extensions.DependencyInjection;
using Pepp.Web.Apps.Bingo.Data.Schemas;

namespace Pepp.Web.Apps.Bingo.Data
{
    /// <summary>
    /// Data service used to interface with the various
    /// tables storing data for the Pepp Bingo App
    /// </summary>
    public interface IBingoDataService
    {
        /// <inheritdoc cref="IApiSchema"/>
        IApiSchema Api { get; }
    }

    /// <inheritdoc cref="IBingoDataService"/>
    public class BingoDataService : BaseDataService, IBingoDataService
    {
        private IApiSchema _apiSchema;

        public BingoDataService(string connStr) : base(connStr)
        {
        }

        public IApiSchema Api { get => _apiSchema ??= new ApiSchema(this); }
    }

    public static class BingoDataServiceExtensions
    {
        /// <summary>
        /// Registers the BingoDataService with the DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connStr">
        ///     The connection string used by the
        ///     data service to connect to the database
        /// </param>
        public static void AddPracticeAPIDataService(this IServiceCollection services, string connStr) =>
            services.AddScoped<IBingoDataService, BingoDataService>(_ => new BingoDataService(connStr));
    }
}
