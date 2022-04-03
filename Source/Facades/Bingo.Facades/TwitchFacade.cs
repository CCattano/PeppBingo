using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiSecretSources =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants
    .ValueDetails.Api.ApiSecrets.Sources;

namespace Pepp.Web.Apps.Bingo.Facades
{
    /// <summary>
    /// Facade for fetching information we store that is
    /// explicitly related to our Twitch sign-in integration
    /// </summary>
    public interface ITwitchFacade
    {
        /// <summary>
        /// Fetches the API secrets required by the
        /// TwitchClient to make request to the Twitch API
        /// </summary>
        /// <returns></returns>
        Task GetTwitchAPISecrets();
    }

    /// <inheritdoc cref="ITwitchFacade"/>
    public class TwitchFacade : ITwitchFacade
    {
        private readonly IBingoDataService _dataSvc;

        public TwitchFacade(IBingoDataService dataSvc) => _dataSvc = dataSvc;

        public async Task GetTwitchAPISecrets()
        {
            // TODO make BE
            // TODO Automapper
            List<ValueDetailDesc> valueDetailDescriptions = 
                await _dataSvc.Api.SecretValueDetailDescRepo.GetValueDetailDescriptions(ApiSecretSources.Twitch);
        }
    }
}
