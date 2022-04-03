using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.Facades;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Adapters
{
    /// <summary>
    /// Adapter responsible for the business logic surrounding Twitch auth flow
    /// </summary>
    public interface ITwitchAdapter
    {
        /// <summary>
        /// Using a access_code provided by twitch generate an access_token for a user
        /// </summary>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        Task<TwitchAccessToken> GetAccessToken(string accessCode);
        /// <summary>
        /// Temporary
        /// </summary>
        /// <returns></returns>
        Task DbFetchTest();
    }

    public class TwitchAdapter : ITwitchAdapter
    {
        private readonly ITwitchFacade _twitchFacade;
        private readonly ITwitchClient _twitchClient;

        public TwitchAdapter(ITwitchFacade twitchFacade, ITwitchClient twitchClient)
        {
            _twitchFacade = twitchFacade;
            _twitchClient = twitchClient;
        }

        public async Task<TwitchAccessToken> GetAccessToken(string accessCode)
        {
            // Make this async Task<string> and store the access token in the Db
            // Use the token string to create a JWT to return to the controller
            // that would be added to the response header for the front end to grab
            TwitchAccessToken accessToken = await _twitchClient.GetAccessToken(accessCode);
            return accessToken;
        }

        public async Task DbFetchTest()
        {
            List<ApiSecretValueDetailDescBE> apiSecrets = await _twitchFacade.GetTwitchAPISecrets();
        }
    }
}
