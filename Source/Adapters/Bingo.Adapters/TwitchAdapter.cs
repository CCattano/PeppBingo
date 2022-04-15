using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Facades;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;
using Pepp.Web.Apps.Bingo.Managers;
using System.Net;
using System.Threading.Tasks;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

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
        Task<string> ProcessReceivedAccessCode(string accessCode);
        /// <summary>
        /// Refreshes the access token of a user via the Twitch API
        /// </summary>
        /// <param name="obsoleteToken"></param>
        /// <returns></returns>
        Task<string> RefreshAccessTokenForUser(string obsoleteToken);
    }

    public class TwitchAdapter : ITwitchAdapter
    {
        private readonly IMapper _mapper;
        private readonly IUserFacade _userFacade;
        private readonly ITwitchFacade _twitchFacade;
        private readonly ITwitchClient _twitchClient;
        private readonly ITokenManager _tokenManager;

        public TwitchAdapter(
            IMapper mapper,
            IUserFacade userFacade,
            ITwitchFacade twitchFacade,
            ITwitchClient twitchClient,
            ITokenManager tokenManager
        )
        {
            _mapper = mapper;
            _userFacade = userFacade;
            _twitchFacade = twitchFacade;
            _twitchClient = twitchClient;
            _tokenManager = tokenManager;
        }

        public async Task<string> ProcessReceivedAccessCode(string accessCode)
        {
            TwitchAccessToken accessToken = await _twitchClient.GetAccessToken(accessCode);
            TwitchUser twitchUser = await _twitchClient.GetUser(accessToken.AccessToken);
            UserBE userBE = _mapper.Map<UserBE>(twitchUser);
            AccessTokenBE accessTokenBE =
                _mapper.Map(accessToken, new AccessTokenBE(userBE.TwitchUserID));
            await _twitchFacade.PersistAccessToken(accessTokenBE);
            userBE = await _userFacade.PersistUser(userBE);
            string token = _tokenManager.GenerateJWTForUser(userBE.UserID);
            return token;
        }

        public async Task<string> RefreshAccessTokenForUser(string obsoleteToken)
        {
            int userID = TokenManager.GetUserIDFromToken(obsoleteToken);
            UserBE userBE = await _userFacade.GetUser(userID);
            AccessTokenBE obsoleteAccessTokenBE = await _twitchFacade.GetAccessToken(userBE.TwitchUserID);
            TwitchAccessToken obsoleteTwitchAccessToken = _mapper.Map<TwitchAccessToken>(obsoleteAccessTokenBE);
            TwitchAccessToken newTwitchAccessToken = await _twitchClient.RefreshAccessToken(obsoleteTwitchAccessToken);
            if (newTwitchAccessToken == null)
                throw new WebException(HttpStatusCode.Forbidden, "Twitch Verification failed");
            AccessTokenBE accessTokenBE =
                _mapper.Map(newTwitchAccessToken, new AccessTokenBE(userBE.TwitchUserID));
            await _twitchFacade.PersistAccessToken(accessTokenBE);
            string token = _tokenManager.GenerateJWTForUser(userBE.UserID);
            return token;
        }
    }
}
