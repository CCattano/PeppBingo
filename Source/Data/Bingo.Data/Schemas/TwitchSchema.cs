using Pepp.Web.Apps.Bingo.Data.Repos.Twitch;

namespace Pepp.Web.Apps.Bingo.Data.Schemas
{
    /// <summary>
    /// Interface containing repos to access the tables associated with the twitch schema
    /// </summary>
    public interface ITwitchSchema
    {
        /// <inheritdoc cref="IAccessTokenRepo"/>
        IAccessTokenRepo AccessTokenRepo { get; }
    }

    /// <inheritdoc cref="ITwitchSchema"/>
    public class TwitchSchema : ITwitchSchema
    {
        private readonly BaseDataService _dataSvc;
        private IAccessTokenRepo _accessTokenRepo;

        public TwitchSchema(BaseDataService dataSvc) => _dataSvc = dataSvc;

        public IAccessTokenRepo AccessTokenRepo { get => _accessTokenRepo ??= new AccessTokenRepo(_dataSvc); }

    }
}
