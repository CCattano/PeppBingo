using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using Pepp.Web.Apps.Bingo.Data.Entities.Twitch;
using Pepp.Web.Apps.Bingo.Infrastructure.Managers.Caches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiSecretSources =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.ApiSecrets.Sources;
using TwitchSecretTypes =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.ApiSecrets.Types.Twitch;

namespace Pepp.Web.Apps.Bingo.Facades
{
    /// <summary>
    /// Facade for working with the information we store that is
    /// explicitly related to our Twitch sign-in integration
    /// </summary>
    public interface ITwitchFacade
    {
        /// <summary>
        /// Ensures the in-memory cache has been hydrated with the Twitch API Secrets
        /// </summary>
        /// <returns></returns>
        Task VerifyTwitchAPISecretsCache();
        /// <summary>
        /// Updates an existing token if it exists otherwise inserts it
        /// </summary>
        /// <param name="tokenBE"></param>
        /// <returns></returns>
        Task PersistAccessToken(AccessTokenBE tokenBE);
    }

    /// <inheritdoc cref="ITwitchFacade"/>
    public class TwitchFacade : ITwitchFacade
    {
        private readonly IMapper _mapper;
        private readonly ITwitchCacheManager _cacheManager;
        private readonly IBingoDataService _dataSvc;

        public TwitchFacade(
            IMapper mapper,
            ITwitchCacheManager cacheManager,
            IBingoDataService dataSvc
        )
        {
            _mapper = mapper;
            _cacheManager = cacheManager;
            _dataSvc = dataSvc;
        }

        public async Task VerifyTwitchAPISecretsCache()
        {
            string clientID = _cacheManager.GetApiSecret(TwitchSecretTypes.ClientID);
            string clientSecret = _cacheManager.GetApiSecret(TwitchSecretTypes.ClientSecret);

            if (!string.IsNullOrWhiteSpace(clientID) && !string.IsNullOrWhiteSpace(clientSecret))
                return;

            List<SecretEntity> valueDetailDescriptions =
               await _dataSvc.Api.SecretRepo.GetSecrets(ApiSecretSources.Twitch);

            Dictionary<TwitchSecretTypes, string> apiSecretsByType =
                valueDetailDescriptions.ToDictionary(key => Enum.Parse<TwitchSecretTypes>(key.Type), value => value.Value);

            _cacheManager.SetApiSecret(TwitchSecretTypes.ClientID, apiSecretsByType[TwitchSecretTypes.ClientID]);
            _cacheManager.SetApiSecret(TwitchSecretTypes.ClientSecret, apiSecretsByType[TwitchSecretTypes.ClientSecret]);
        }

        public async Task PersistAccessToken(AccessTokenBE tokenBE)
        {
            // Look for existing token by TwitchUserID
            AccessTokenEntity tokenEntity = await _dataSvc.Twitch.AccessTokenRepo.GetAccessToken(tokenBE.TwitchUserID);
            if (tokenEntity == null)
            {
                // Token does not exist, make new entity from BE data
                tokenEntity = _mapper.Map<AccessTokenEntity>(tokenBE);
                // Insert BE data into Db
                await _dataSvc.Twitch.AccessTokenRepo.InsertAccessToken(tokenEntity);
                return;
            }
            // Map BE values onto Entity from Db to update entity contents
            tokenEntity = _mapper.Map(tokenBE, tokenEntity);
            // Post latest contents to Db
            await _dataSvc.Twitch.AccessTokenRepo.UpdateAccessToken(tokenEntity);
        }
    }
}
