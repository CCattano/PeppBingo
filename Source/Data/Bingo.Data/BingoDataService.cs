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

        /// <inheritdoc cref="IGameSchema"/>
        IGameSchema Game { get; }

        /// <inheritdoc cref="IStatsSchema"/>
        IStatsSchema Stats { get; }
    }

    /// <inheritdoc cref="IBingoDataService"/>
    public class BingoDataService : BaseDataService, IBingoDataService
    {
        private IApiSchema _apiSchema;
        private ITwitchSchema _twitchSchema;
        private IUserSchema _userSchema;
        private ITokenSchema _tokenSchema;
        private IGameSchema _gameSchema;
        private IStatsSchema _statsSchema;

        public BingoDataService(string connStr) : base(connStr)
        {
        }

        public IApiSchema Api => _apiSchema ??= new ApiSchema(this);
        public ITwitchSchema Twitch => _twitchSchema ??= new TwitchSchema(this);
        public IUserSchema User => _userSchema ??= new UserSchema(this);
        public ITokenSchema Token => _tokenSchema ??= new TokenSchema(this);
        public IGameSchema Game => _gameSchema ??= new GameSchema(this);
        public IStatsSchema Stats => _statsSchema ??= new StatsSchema(this);
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
        public static void AddBingoDataService(this IServiceCollection services, string connStr) =>
            services.AddScoped<IBingoDataService, BingoDataService>(_ => new BingoDataService(connStr));
    }
}