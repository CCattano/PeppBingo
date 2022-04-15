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

        /// <inheritdoc cref="ITwitchSchema"/>
        ITwitchSchema Twitch { get; }

        /// <inheritdoc cref="IUserSchema"/>
        IUserSchema User { get; }
        /// <inheritdoc cref="ITokenSchema"/>
        ITokenSchema Token { get; }
    }

    /// <inheritdoc cref="IBingoDataService"/>
    public class BingoDataService : BaseDataService, IBingoDataService
    {
        private IApiSchema _apiSchema;
        private ITwitchSchema _twitchSchema;
        private IUserSchema _userSchema;
        private ITokenSchema _tokenSchema;

        public BingoDataService(string connStr) : base(connStr)
        {
        }

        public IApiSchema Api { get => _apiSchema ??= new ApiSchema(this); }
        public ITwitchSchema Twitch { get => _twitchSchema ??= new TwitchSchema(this); }
        public IUserSchema User { get => _userSchema ??= new UserSchema(this); }
        public ITokenSchema Token { get => _tokenSchema ??= new TokenSchema(this); }

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
