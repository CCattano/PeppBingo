using Pepp.Web.Apps.Bingo.Facades;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;
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
        Task ProcessReceivedAccessCode(string accessCode);
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

        public async Task ProcessReceivedAccessCode(string accessCode)
        {
            // TODO store token data
            // TODO store user data
            // TODO implement JWT management
            // TODO return Task<string> w/ JWT to be written to headers
            await _twitchFacade.VerifyTwitchAPISecretsCache();
            TwitchAccessToken accessToken = await _twitchClient.GetAccessToken(accessCode);
            TwitchUser user = await _twitchClient.GetUser(accessToken.AccessToken);
        }
    }
}
