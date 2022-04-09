using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
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
        private readonly IMapper _mapper;
        private readonly IUserFacade _userFacade;
        private readonly ITwitchFacade _twitchFacade;
        private readonly ITwitchClient _twitchClient;

        public TwitchAdapter(
            IMapper mapper,
            IUserFacade userFacade,
            ITwitchFacade twitchFacade,
            ITwitchClient twitchClient
        )
        {
            _mapper = mapper;
            _userFacade = userFacade;
            _twitchFacade = twitchFacade;
            _twitchClient = twitchClient;
        }

        public async Task ProcessReceivedAccessCode(string accessCode)
        {
            await _twitchFacade.VerifyTwitchAPISecretsCache();
            TwitchAccessToken accessToken = await _twitchClient.GetAccessToken(accessCode);
            TwitchUser twitchUser = await _twitchClient.GetUser(accessToken.AccessToken);
            UserBE userBE = _mapper.Map<UserBE>(twitchUser);
            AccessTokenBE accessTokenBE = 
                _mapper.Map(accessToken, new AccessTokenBE(userBE.TwitchUserID));
            await _twitchFacade.PersistAccessToken(accessTokenBE);
            userBE = await _userFacade.PersistUser(userBE);
            // TODO set up JWT management logic
            // TODO return redirect response w/ JWT to be written to headers
        }
    }
}
