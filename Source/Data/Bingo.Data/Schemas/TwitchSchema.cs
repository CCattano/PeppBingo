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
    public class TwitchSchema : BaseSchema, ITwitchSchema
    {
        private IAccessTokenRepo _accessTokenRepo;

        public TwitchSchema(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public IAccessTokenRepo AccessTokenRepo { get => _accessTokenRepo ??= new AccessTokenRepo(base.DataSvc); }

    }
}
