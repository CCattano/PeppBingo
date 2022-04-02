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
        Task<TwitchAccessToken> GetAccessToken(string accessCode);
    }

    public class TwitchAdapter : ITwitchAdapter
    {
        private readonly ITwitchClient _twitchClient;

        public TwitchAdapter(ITwitchClient twitchClient)
        {
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
    }
}
