using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Facades;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;
using Pepp.Web.Apps.Bingo.Managers;
using System;
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
        Task<string> ProcessReceivedAccessCode(string accessCode);
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
            string token = _tokenManager.GenerateJWTForUser(userBE.UserID, DateTime.Now.AddDays(1));
            return token;
        }
    }
}
