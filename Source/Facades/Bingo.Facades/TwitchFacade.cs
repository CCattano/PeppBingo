using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using System.Collections.Generic;
using System.Linq;
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
        Task<List<ApiSecretValueDetailDescBE>> GetTwitchAPISecrets();
    }

    /// <inheritdoc cref="ITwitchFacade"/>
    public class TwitchFacade : ITwitchFacade
    {
        private readonly IMapper _mapper;
        private readonly IBingoDataService _dataSvc;

        public TwitchFacade(IMapper mapper, IBingoDataService dataSvc)
        {
            _mapper = mapper;
            _dataSvc = dataSvc;
        }

        public async Task<List<ApiSecretValueDetailDescBE>> GetTwitchAPISecrets()
        {
            List<ValueDetailDescEntity> valueDetailDescriptions = 
                await _dataSvc.Api.SecretValueDetailDescRepo.GetValueDetailDescriptions(ApiSecretSources.Twitch);

            List<ApiSecretValueDetailDescBE> apiSecrets =
                valueDetailDescriptions.Select(vdd => _mapper.Map<ApiSecretValueDetailDescBE>(vdd)).ToList();

            return apiSecrets;
        }
    }
}
